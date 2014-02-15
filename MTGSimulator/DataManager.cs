using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

using System.IO;

using log4net;

namespace MTGUtils
{
    public class DataManager:IDisposable
    {
        private readonly ILog log;

        // For Holding Magic Card Data in memory
        private List<MTGSet> Sets;
        private DateTime LastSetsUpdate { get; set; }

        // For accessing/storing of card data
        private string mainURL = "http://www.mtgprice.com/magic-the-gathering-prices.jsp";
        private string startURL = "http://www.mtgprice.com";
        private SQLWrapper _SQLWrapper;
        private HTMLParser _HTMLParser;

        // For saving/loading of application state data
        private MTGUtils.AppState _ApplicationState;

        public DataManager()
        {
            _SQLWrapper = new SQLWrapper();
            _HTMLParser = new HTMLParser();
            _ApplicationState = new MTGUtils.AppState();

            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Sets = _SQLWrapper.GetSetList();
        }

        /* URL fetching/parsing for sets */
        public void UpdateSetURLs()
        {
            URLFetcher Fetcher = new URLFetcher(mainURL);
            string ret = Fetcher.Fetch();

            Sets = _HTMLParser.ParseSets(ret);

            _SQLWrapper.UpdateSetList(Sets);

            Sets = Sets.OrderBy(set => set.ToString()).ToList();
            LastSetsUpdate = DateTime.Today;
        }

        /* URL fetching/parsing for cards for a given set*/
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

        /* Updating the application state to be stored */
        public void UpdateAppState(List<int> CheckedPriceSources, List<int> CheckedMTGSets)
        {
            _ApplicationState.UpdateAppState(CheckedPriceSources, CheckedMTGSets);
        }

        public void GetAppState(ref List<int> CheckedPriceSources, ref List<int> CheckedMTGSets)
        {
            _ApplicationState.GetAppState(ref CheckedPriceSources, ref CheckedMTGSets);
        }

        public void Dispose()
        {
            _ApplicationState.Dispose();
        }

        public List<MTGSet> GetSets()
        {
            return Sets;
        }
    }
}
