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

        public List<MTGSet> ParseSets(string htmlIn)
        {
            List<MTGSet> retSets = new List<MTGSet>();
            retSets.Clear();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlIn.ToString());

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
                            string setName = lineNode.InnerText;
                            string setURL = lineNode.FirstChild.GetAttributeValue("href", null);

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
                                DateTime lastUpdate = new DateTime(); // Unset as we do not yet know last update time.

                                MTGSet tempSet = new MTGSet(setName, setDate, lastUpdate);
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
    }
}
