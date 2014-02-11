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
        private DateTime LastSetsUpdate { get; set; }
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
            LastSetsUpdate = DateTime.Today;
        }

        public void GetCardListForSet(string setName)
        {
            MTGSet curSet = null;
            foreach (MTGSet set in Sets)
            {
                if (set.SetName.CompareTo(setName) == 0)
                {
                    curSet = set;
                    break;
                }
            }

            if (curSet == null)
            {
                log.Error("Could not find Set named '" + setName + "'");
                return;
            }

            if (curSet.CardListLastUpdate.CompareTo(DateTime.Today) < 0)
            {
                URLFetcher Fetcher = new URLFetcher(startURL + curSet.URL);
                string ret = Fetcher.Fetch();

                List<MTGCard> Cards;
                Cards = _HTMLParser.ParseCardURLs(ret);
                Cards = Cards.OrderBy(card => card.ToString()).ToList();

                curSet.CardListLastUpdate = DateTime.Today;
            }
        }

        public List<MTGSet> GetSets()
        {
            return Sets;
        }
    }
}
