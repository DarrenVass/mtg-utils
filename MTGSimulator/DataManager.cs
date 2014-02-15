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
        public List<MTGCard> GetCardListForSet(string setName)
        {
            MTGSet curSet = null;
            List<MTGCard> curCards = null;
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
                return null;
            }

            if (curSet.CardListLastUpdate.CompareTo(DateTime.Today) < 0)
            {
                // Need to Update List
                URLFetcher Fetcher = new URLFetcher(startURL + curSet.URL);
                string ret = Fetcher.Fetch();

                curCards = _HTMLParser.ParseCardURLs(ret, setName);
                curCards = curCards.OrderBy(card => card.ToString()).ToList();

                curSet.CardListLastUpdate = DateTime.Today;

                _SQLWrapper.UpdateCardList(curCards, setName);
                _SQLWrapper.UpdateSetLastUpdate(setName, curSet.CardListLastUpdate);
            }
            else
            {
                // List is already up to date, parse from SQL
                curCards = _SQLWrapper.GetCardList(setName);
            }

            curCards = curCards.OrderByDescending(card => card.Price).ToList();

            return curCards;
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
