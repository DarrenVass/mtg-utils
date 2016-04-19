using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

namespace MTGUtils
{
    class FormatParser
    {
        private readonly ILog log;

        public string StandardFormatName = "STANDARD";
        public string StandardURL { get; } = "http://magic.wizards.com/en/content/standard-formats-magic-gathering";
        public string ModernFormatName = "MODERN";
        public string ModernURL { get; } = "http://magic.wizards.com/en/gameinfo/gameplay/formats/modern";


        public FormatParser()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /* Parsing of all sets currenlty in Standard */
        /* http://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
         * Apparently <option> tags are expected to have end tags, but HTMLAgilityPack
         *  thinks they shouldn't and so causes errors. So there is a change of option to my_option to get around this.
         */
        private List<string> ParseStandard(string HTMLIn)
        {
            HTMLIn = HTMLIn.Replace("<option", "<my_option");
            HTMLIn = HTMLIn.Replace("</option>", "</my_option>");

            // Double check formatting
            if (HTMLIn.Contains("standard-html") == false)
            {
                log.Error(StandardURL + " formatting has been changed. Need to update the application.");
                return null;
            }

            List<string> ret = new List<string>();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(HTMLIn.ToString());

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                foreach (HtmlAgilityPack.HtmlParseError err in htmlDoc.ParseErrors)
                {
                    log.Error("HTMLAgilityPack Error: " + err.Code + " " + err.Reason + " " + err.SourceText + " " + err.Line );
                }
                return null;
            }

            if (htmlDoc.DocumentNode == null)
            {
                log.Error("FormatParser::ParseStandard(): DocumentNode is null");
                return null;
            }
            HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[@style = 'opacity: 1;']");

            if (bodyNode == null)
            {
                log.Error("FormatParser::ParseStandard(): bodyNode is null");
                return null;
            }

            foreach (HtmlAgilityPack.HtmlNode childNode in bodyNode.ChildNodes)
            {
                if (childNode == null)
                    continue;
                foreach (HtmlAgilityPack.HtmlNode emNode in childNode.ChildNodes)
                {
                    if (emNode.Name == "em")
                    {
                        ret.Add(emNode.InnerText);
                    }
                }
            }

            return ret;
        }

        /* Parsing of all sets currenlty in Modern */
        /* http://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
         * Apparently <option> tags are expected to have end tags, but HTMLAgilityPack
         *  thinks they shouldn't and so causes errors. So there is a change of option to my_option to get around this.
         */
        private List<string> ParseModern(string HTMLIn)
        {
            HTMLIn = HTMLIn.Replace("<option", "<my_option");
            HTMLIn = HTMLIn.Replace("</option>", "</my_option>");

            // Clean Up setnames to match mtgprice sitenames.
            HTMLIn = HTMLIn.Replace("&nbsp;", ""); // Remove this char
            HTMLIn = HTMLIn.Replace("Magic 20", "M"); // 'Magic 20XX' to 'MXX'
            HTMLIn = HTMLIn.Replace("Dragon's Maze", "Dragons Maze"); // 'Dragons Maze' not 'Dragon's Maze'
            HTMLIn = HTMLIn.Replace(": City of Guilds", ""); // 'Ravnica' not 'Ravnica: City of Guilds'
            HTMLIn = HTMLIn.Replace("Tenth", "10th");
            HTMLIn = HTMLIn.Replace("Ninth", "9th");
            HTMLIn = HTMLIn.Replace("Eighth", "8th"); 


            // Double check formatting
            if (HTMLIn.Contains("<ul class=\"list\">") == false)
            {
                log.Error(ModernURL + " formatting has been changed. Need to update the application.");
                return null;
            }

            List<string> ret = new List<string>();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(HTMLIn.ToString());

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                foreach (HtmlAgilityPack.HtmlParseError err in htmlDoc.ParseErrors)
                {
                    log.Error("HTMLAgilityPack Error: " + err.Code + " " + err.Reason + " " + err.SourceText + " " + err.Line);
                }
                return null;
            }

            if (htmlDoc.DocumentNode == null)
            {
                log.Error("FormatParser::ParseStandard(): DocumentNode is null");
                return null;
            }
            HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class = 'list']");

            if (bodyNode == null)
            {
                log.Error("FormatParser::ParseStandard(): bodyNode is null");
                return null;
            }

            foreach (HtmlAgilityPack.HtmlNode childNode in bodyNode.ChildNodes)
            {
                if (childNode == null)
                    continue;
                foreach (HtmlAgilityPack.HtmlNode emNode in childNode.ChildNodes)
                {
                    if (emNode.Name == "em")
                    {
                        ret.Add(emNode.InnerText);
                    }
                }
            }

            return ret;
        }

        public MTGFormat ParseFormat(string HTMLIn, string FormatName)
        {
            MTGFormat ret = new MTGFormat(FormatName);
            ret.FormatListLastUpdate = DateTime.Today;
            if (string.Compare(FormatName, StandardFormatName, true) == 0)
                { ret.Sets = ParseStandard(HTMLIn); }
            else if (string.Compare(FormatName, ModernFormatName, true) == 0)
                { ret.Sets = ParseModern(HTMLIn); }
            else
            {
                log.Error("FormatParser::ParseFormat() supplied unexpected format name '" + FormatName + "'");
                return null;
            }

            return ret;
        }

    }
}
