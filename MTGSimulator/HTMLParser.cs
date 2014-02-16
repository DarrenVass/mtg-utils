using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml.XPath;

using log4net;

namespace MTGUtils
{
    class HTMLParser
    {
        private readonly ILog log;

        public HTMLParser()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /* Information stored in '<tbody>' */
        public List<MTGSet> ParseSets(string HTMLIn)
        {
            List<MTGSet> retSets = new List<MTGSet>();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(HTMLIn.ToString());

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                foreach (HtmlAgilityPack.HtmlParseError err in htmlDoc.ParseErrors)
                {
                    log.Error("HTMLAgilityPack Error: " + err.ToString());
                }
            }
            else
            {// Success
                if (htmlDoc.DocumentNode != null)
                {
                    HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//tbody");
                    if (bodyNode != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode childNode in bodyNode.ChildNodes)
                        {
                            //FirstNode is URL & Name
                            HtmlAgilityPack.HtmlNode lineNode = childNode.FirstChild;
                            string setName = lineNode.InnerText.TrimEnd(' ');
                            string setURL = lineNode.FirstChild.GetAttributeValue("href", null).TrimEnd(' ');

                            if (setName.Contains("Foil"))
                            { /* Foil Set, just add URL */
                                string setNameWithoutFoil = setName.Replace(" (Foil)", "");
                                bool isFound = false;
                                foreach (MTGSet set in retSets)
                                {
                                    if (set.ToString().CompareTo(setNameWithoutFoil) == 0)
                                    {
                                        set.FoilURL = setURL; 
                                        isFound = true;
                                    }
                                }

                                if (!isFound)
                                {
                                    log.Warn("Unable to find Non-Foil set for '" + setNameWithoutFoil + "'");
                                }
                            }
                            else
                            { /* New Set */
                                //SecondNode is Set Release Date
                                lineNode = lineNode.NextSibling;
                                string date = lineNode.FirstChild.WriteTo();
                                string[] dates = date.Split('/');
                                DateTime setDate = new DateTime(Convert.ToInt16(dates[2]),
                                                                Convert.ToInt16(dates[0]),
                                                                Convert.ToInt16(dates[1]));
 
                                MTGSet tempSet = new MTGSet(setName, setDate);
                                tempSet.URL = setURL;
                                retSets.Add(tempSet);
                            }
                        }
                    }
                    else
                    {
                        log.Error("Body Node Null");
                    }
                                        
                 }
                else 
                { 
                    log.Error("HTMLDoc Node Null");
                }
            }

            return retSets;
        }

        /* Information stored in '<tbody>' */
        public List<MTGCard> ParseCardURLs(string HTMLIn, string SetName)
        {
            List<MTGCard> retCards = new List<MTGCard>();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(HTMLIn.ToString());

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                foreach (HtmlAgilityPack.HtmlParseError err in htmlDoc.ParseErrors)
                {
                    log.Error("HTMLAgilityPack Error: " + err.ToString());
                }
            }
            else
            {// Success
                if (htmlDoc.DocumentNode != null)
                {
                    HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//tbody");
                    if (bodyNode != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode childNode in bodyNode.ChildNodes)
                        {
                            //FirstNode is URL & Name

                            HtmlAgilityPack.HtmlNode lineNode = childNode.FirstChild;
                            if (lineNode == null) { continue; }

                            string cardName = lineNode.InnerText.TrimEnd(' ');
                            string setURL = lineNode.FirstChild.GetAttributeValue("href", null).TrimEnd(' ').TrimEnd(' ');
                            string price = lineNode.NextSibling.InnerText;
                            
                            // There are some duplicated entries where the 2nd has price of "%N/A", giving same URL's. Ignore them
                            if (price.CompareTo("$N/A") == 0) { continue; }

                            // Remove '$' and '.' and make sure cents are 2 digits.
                            price = price.Replace("$", string.Empty);

                            if(price.Contains('.'))
                            {
                                // Price can be $1.23 or $1.2 when it should be $1.20, so need to fix that case.
                                if (price.Length - price.LastIndexOf('.') == 2)
                                {
                                    price += "0";
                                }
                                price = price.Replace(".", string.Empty);
                            }
                            else
                            { // Price like $4 when should be $4.00
                                price = price + "00";
                            }

                            MTGCard tempCard = new MTGCard(cardName, SetName, Convert.ToUInt64(price));
                            tempCard.URL = setURL;
                            retCards.Add(tempCard);
                        }
                    }
                    else
                    {
                        log.Error("Body Node Null");
                    }

                }
                else
                {
                    log.Error("HTMLDoc Node Null");
                }
            }

            return retCards;
        }

        /* JavaScript is used to populate the tbody, and the formatting on the data is odd. So pretty much just have to search for data */
        public List<PricePoint> ParsePricePoints(string HTMLIn, MTGCard CardIn)
        {
            List<PricePoint> retPP = new List<PricePoint>();

            // Double check formatting
            if(HTMLIn.Contains("var results = [{") == false || HTMLIn.Contains("var sellPriceData = [") == false)
            {
                log.Error("MTGPrice.com PricePoints formatting has been changed. Need to update the application.");
                return null;
            }

            int start = HTMLIn.IndexOf("var results = [{");
            int end = HTMLIn.IndexOf("var sellPriceData = [");

            if (start == -1 || end == -1)
            {
                log.Error("MTGPrice.com PricePoints formatting has been changed. Need to update the application. (start = " + start + " end = " + end + ")");
                return null;
            }

            /*
             *  Formatting Example:
             *  {
             *  "color": "rgb(140,172,198)",
             *  "label": "HotSauce Games - $19.99",
             *   "data": [[1379804931252,29.99],[1379964574414,29.99]]},
             *   
             * The above group repeats per Retailer
             */

            string relevantData = HTMLIn.Substring(start, (end-start));

            // Keep going until a '"label":' is not found.

            string labelString = "\"label\": ";
            string dataString = "\"data\": ";
            while (true)
            {
                int labelIndex = relevantData.IndexOf(labelString);
                if (labelIndex < 0)
                {
                    // No more retailers, done!
                    break;
                }

                int commaIndex = relevantData.IndexOf(',', labelIndex);
                string retailerName = relevantData.Substring((labelIndex + labelString.Length), commaIndex - labelIndex - labelString.Length);
                // Format is now '"<NAME>" - $<PRICE>', remove quotes, and everything after name.
                retailerName = retailerName.Substring(1, retailerName.IndexOf(" -") - 1);

                // Start at Index 1, as the 2nd-Xth strings will be "}DATA"
                int endIndex = relevantData.IndexOf('}', 1);

                int dataIndex = relevantData.IndexOf(dataString);

                // Put just the price points into [DATE,PRICE],[DATE,PRICE] format and 
                // Trim the first/last '[' and ']'
                string pureData = relevantData.Substring(dataIndex + dataString.Length , endIndex - dataIndex - dataString.Length);
                // Split based on delimiter "],[" and use "[[" and "]]" to remove first/last double brackets and then remove emptry entries
                // The "[]" case takes care of empty brackets
                string[] delim = new string[] { "],[", "[[", "]]", "[]" };
                string [] splitData = pureData.Split(delim, StringSplitOptions.RemoveEmptyEntries);

                // Data is now formatted as '[DATE,PRICE]'
                foreach (string point in splitData)
                {
                    PricePoint tempPP = new PricePoint();
                    tempPP.Retailer = retailerName;

                    // Convert from Milliseconds since Unix Epoch to DateTime
                    string[] splitPoint = point.Split(',');
                    tempPP.Date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    tempPP.Date = tempPP.Date.AddSeconds(Convert.ToDouble(splitPoint[0]) / 1000).ToLocalTime();
                    tempPP.Price = Convert.ToUInt64(splitPoint[1].Replace(".", ""));
                    retPP.Add(tempPP);
                }

                relevantData = relevantData.Substring(endIndex);
            }

            return retPP;
        }
    }
}
