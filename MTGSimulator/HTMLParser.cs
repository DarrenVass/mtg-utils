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

        /* Information stored in the second '<tbody>' */
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
                return null;
            }

            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("(//tbody)[2]");

                if (bodyNode != null)
                {
                    foreach (HtmlAgilityPack.HtmlNode childNode in bodyNode.ChildNodes)
                    {
                        if (childNode == null)
                            continue;
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

            return retSets;
        }

        /* Information stored in <script> and specific line has "$scope.setList" */
        public List<MTGCard> ParseCardURLs(string HTMLIn, string SetName)
        {
            List<MTGCard> retCards = new List<MTGCard>();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(HTMLIn.ToString());
            string result = htmlDoc.DocumentNode.OuterHtml;

            /* As they use a script to display card names/urls/etc no easy way to get the specified line. So big giant string with entire HTML doc inside. */

            int listLocation = result.IndexOf("$scope.setList");
            int startOfList = result.IndexOf("[{", listLocation);
            int endOfList = result.IndexOf("}];", startOfList);

            // Off by 2 to remove the '[{'
            string listOfCards = result.Substring(startOfList + 2, (endOfList - startOfList - 2));

            /* Now we have listOfCards which is of the format:
             * 
             * {"cardId":"AEtherlingDragons_MazefalseNM-M","name":"AEtherling","quantity":0,"countForTrade":0,"isFoil":false,"url":"/sets/Dragons_Maze/AEtherling",
             * "setUrl":"/spoiler_lists/Dragons_Maze","fair_price":0.39,"setName":"Dragons Maze","absoluteChangeSinceYesterday":0.0,"absoluteChangeSinceOneWeekAgo":0.0,
             * "percentageChangeSinceYesterday":0.0,"percentageChangeSinceOneWeekAgo":0.0,"color":"U","rarity":"R","manna":"4UU","bestVendorBuylist":"UNDEFINED",
             * "bestVendorBuylistPrice":"0","lowestPrice":"0.33","lowestPriceVendor":"HotSauce Games",
             * "fullImageUrl":"http://s.mtgprice.com/sets/Dragons_Maze/img/AEtherling.full.jpg"},{},{}
             */

            string[] stringSeperator = new string[] {"},{"};
            listOfCards = listOfCards.Replace("\\u0027", "'");
            string[] cardStrings = listOfCards.Split(stringSeperator, StringSplitOptions.None);

            // Now we have each card {INFO} in a string

            foreach (string cardString in cardStrings)
            {
                // Now to break it up into "Tag":"Value" pairs. The value can be numerical as well.
                string[] infoStringSeperator = new string[] {"\",\"", ",\"" };
                string[] infoStrings = cardString.Split(infoStringSeperator, StringSplitOptions.None);
                string cardName = null, setURL = null, price = null;

                foreach (string infoString in infoStrings)
                {
                    // Now in the format of "TAG":"VALUE"
                    string[] cleanStringSeperator = new string[] { "\":" };
                    string[] tagStrings = infoString.Split(cleanStringSeperator, StringSplitOptions.None);
                    if (tagStrings.Count() != 2)
                        continue;
                    if (tagStrings[0] == "name")
                    {
                        cardName = tagStrings[1].Trim('"');
                    }
                    else if (tagStrings[0] == "url")
                    {
                        setURL = tagStrings[1].Trim('"');
                    }
                    else if (tagStrings[0] == "fair_price")
                    {
                        price = tagStrings[1].Trim('"');
                        if (price.Contains('.'))
                        {
                            if (price.Length - price.LastIndexOf('.') == 2) // Adjust "1.X" to "1.X0"
                                price += "0";
                            else if (price.Length - price.LastIndexOf('.') > 3) // Adjust "1.XXxxxxxx" to "1.XX"
                                price = price.Substring(0, price.LastIndexOf('.') + 3);
                            price = price.Replace(".", string.Empty);
                        }
                        else
                        {
                            price = price + "00"; // Adjust "X" to "X00"
                        }
                        
                    }
                }

                MTGCard tempCard = new MTGCard(cardName, SetName, Convert.ToUInt64(price));
                tempCard.URL = setURL;
                retCards.Add(tempCard);
            }
            
            return retCards;
        }

        /*
         * Called to save the URL of the card image.
         * Format of: 
         * <meta property = "og:image" content="http://s.mtgprice.com/sets/Magic_Origins/img/Jace, Vryn's Prodigy.full.jpg" />
         */
        private void ParseCardImage(string HTMLIn, MTGCard CardIn)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(HTMLIn.ToString());
            HtmlAgilityPack.HtmlNode imageNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property = 'og:image']");
            
            foreach(HtmlAgilityPack.HtmlAttribute att in imageNode.Attributes)
            {
                if(att.Name == "content")
                {
                    CardIn.CardImageURL = att.Value;
                }
            }           
        }


        /* 
         * JavaScript is used to populate the tbody, and the formatting on the data is odd, 
         * so pretty much just have to search for data and parse manually. 
         * Ignore all zero priced data as it's useless.
         */
        public List<PricePoint> ParsePricePoints(string HTMLIn, MTGCard CardIn)
        {
            List<PricePoint> retPP = new List<PricePoint>();

            // Double check formatting
            if(HTMLIn.Contains("var results = [") == false || HTMLIn.Contains("var sellPriceData = [") == false)
            {
                log.Error("MTGPrice.com PricePoints formatting has been changed. Need to update the application.");
                return null;
            }

            if(CardIn.CardImageURL == null || CardIn.CardImageURL == "")
            {
                ParseCardImage(HTMLIn, CardIn);
            }

            int start = HTMLIn.IndexOf("var results = [");
            int end = HTMLIn.IndexOf("var sellPriceData = [");

            if (start == -1 || end == -1)
            {
                log.Error("MTGPrice.com PricePoints formatting has been changed. Need to update the application. (start = " + start + " end = " + end + ")");
                return null;
            }

            /*
             *  Formatting Example:
             *  {
             *      "color": "rgb(140,172,198)",
             *      "label": "HotSauce Games - $19.99",
             *      "lines": { "show": true, "fill": true },
             *      "data": [[1379804931252,29.99],[1379964574414,29.99]]
             *   },
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
                
                int dataIndex = relevantData.IndexOf(dataString);
                // Start at Index 1, as the 2nd-Xth strings will be "}DATA"
                int endIndex = relevantData.IndexOf('}', dataIndex);

                // Put just the price points into [DATE,PRICE],[DATE,PRICE] format and 
                // Trim the first/last '[' and ']'
                string pureData = relevantData.Substring(dataIndex + dataString.Length , endIndex - dataIndex - dataString.Length);
                pureData = pureData.Replace(" ", string.Empty);
                pureData = pureData.Replace("\r", string.Empty);
                pureData = pureData.Replace("\n", string.Empty);
                if (pureData != null && pureData != "[]")
                {
                    // Split based on delimiter "],[" and use "[[" and "]]" to remove first/last double brackets and then remove emptry entries
                    // The "[]" case takes care of empty brackets
                    string[] delim = new string[] { "],[", "[[", "]]", "[]" };
                    string[] splitData = pureData.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    // Data is now formatted as '[DATE,PRICE]'
                    foreach (string point in splitData)
                    {
                        string[] splitPoint = point.Split(',');
                        string stringPrice = splitPoint[1];

                        if (stringPrice == "0.0" || stringPrice == "0") // Ignore zero prices.
                        {
                            continue;
                        }

                        if (stringPrice.Contains('.'))
                        {
                            if (stringPrice.Length - stringPrice.LastIndexOf('.') == 2) // Adjust "1.X" to "1.X0"
                                stringPrice += "0";
                            else if (stringPrice.Length - stringPrice.LastIndexOf('.') > 3) // Adjust "1.XXxxxxxx" to "1.XX"
                                stringPrice = stringPrice.Substring(0, stringPrice.LastIndexOf('.') + 3);
                            stringPrice = stringPrice.Replace(".", string.Empty);
                        }
                        else
                        {
                            stringPrice = stringPrice + "00"; // Adjust "X" to "X00"
                        }

                        UInt64 Price = Convert.ToUInt64(stringPrice);

                        if(Price == 0)
                          log.Error(Price + " from '" + stringPrice + "'");

                        PricePoint tempPP = new PricePoint();
                        tempPP.Retailer = retailerName;
                        // Convert from Milliseconds since Unix Epoch to DateTime
                        tempPP.Date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        tempPP.Date = tempPP.Date.AddSeconds(Convert.ToDouble(splitPoint[0]) / 1000).ToLocalTime();
                        tempPP.Price = Price;
                        retPP.Add(tempPP);
                    }
                }

                relevantData = relevantData.Substring(endIndex);
            }

            return retPP;
        }

    }
}
