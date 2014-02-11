using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

using System.IO;

using log4net;

namespace MTGUtils
{
    class DataManager
    {
        private List<MTGSet> Sets;
        private SQLWrapper _SQLWrapper;
        private HTMLParser _HTMLParser;

        private readonly ILog log;
        private string mainURL = "http://www.mtgprice.com/magic-the-gathering-prices.jsp";
        private string startURL = "http://www.mtgprice.com";

        public DataManager()
        {
            _SQLWrapper = new SQLWrapper();
            _HTMLParser = new HTMLParser(); 

            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Sets = _SQLWrapper.GetSetList();
        }

        ~DataManager()
        {
            //_SQLWrapper
        }

        /* Download Set List, Parse It, then save it. */
        public void UpdateSetURLs()
        {
            URLFetcher Fetcher = new URLFetcher(mainURL);
            string ret = Fetcher.Fetch();

            Sets = _HTMLParser.ParseSets(ret);

            _SQLWrapper.UpdateSetList(Sets);

            Sets = Sets.OrderBy(set => set.ToString()).ToList();
        }

        public void UpdateURLsForSet(string setName)
        {
            string setURL = null;
            foreach (MTGSet set in Sets)
            {
                if (set.SetName.CompareTo(setName) == 0)
                {
                    setURL = set.URL;
                    break;
                }
            }

            if (setURL == null)
            {
                log.Error("Could not find SetURL for set named '" + setName + "'");
                return;
            }

            log.Debug("Updating CardURLs for : " + startURL + setURL);
            URLFetcher Fetcher = new URLFetcher(startURL + setURL);
            string ret = Fetcher.Fetch();

            List<string> CardURLs;
            CardURLs = _HTMLParser.ParseCardURLs(ret);


        }

        public List<MTGSet> GetSets()
        {
            return Sets;
        }
    }
}
