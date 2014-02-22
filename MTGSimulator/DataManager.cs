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
        private List<MTGSet> Sets;      // Information for ALL sets
        private MTGCard CurrentCard;
        private List<PricePoint> CurrentPricePoints;

        private DateTime LastSetsUpdate { get; set; }

        // For accessing/storing of card data
        private string mainURL = "http://www.mtgprice.com/magic-the-gathering-prices.jsp";
        private string startURL = "http://www.mtgprice.com";
        private SQLWrapper _SQLWrapper;
        private HTMLParser _HTMLParser;
        private DataFilter _DataFilter;

        // For saving/loading of application state data
        private MTGUtils.AppState _ApplicationState;

        public DataManager()
        {
            _SQLWrapper = new SQLWrapper();
            _HTMLParser = new HTMLParser();
            _DataFilter = new DataFilter();
            _ApplicationState = new MTGUtils.AppState();

            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Sets = _SQLWrapper.GetSetList();
            CurrentCard = null;
            CurrentPricePoints = null;
        }

        public void Dispose()
        {
            _ApplicationState.Dispose();
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

        /* URL fetching/parsing for cards for a given set */
        public List<MTGCard> GetCardListForSet(MTGSet SetIn)
        {
            if (SetIn == null)
            {
                log.Error("GetCardListForSet supplied null MTGSet");
                return null;
            }
            List<MTGCard> curCards = new List<MTGCard>();
            if (SetIn.CardListLastUpdate.CompareTo(DateTime.Today) < 0)
            {
                // Need to Update List
                URLFetcher Fetcher = new URLFetcher(startURL + SetIn.URL);
                string ret = Fetcher.Fetch();

                curCards = _HTMLParser.ParseCardURLs(ret, SetIn.ToString());
                curCards = curCards.OrderBy(card => card.ToString()).ToList();

                SetIn.CardListLastUpdate = DateTime.Today;

                _SQLWrapper.UpdateCardList(curCards, SetIn.ToString());
                _SQLWrapper.UpdateSetLastUpdate(SetIn.ToString(), SetIn.CardListLastUpdate);
            }
            else
            {
                // List is already up to date, parse from SQL
                curCards = _SQLWrapper.GetCardList(SetIn.ToString());
            }

            curCards = curCards.OrderByDescending(card => card.Price).ToList();

            return curCards;
        }

        /* Price Point fetching/parsing for a particular card/set */
        public List<PricePoint> GetPricePointsForCard(MTGCard CardIn)
        {
            if (CardIn == null)
            {
                log.Error("UpdatePricePoints supplied null MTGCard");
                return null;
            }

            CurrentCard = CardIn;
            List<PricePoint> retPP = new List<PricePoint>();
            if (CardIn.LastPricePointUpdate.CompareTo(DateTime.Today) < 0)
            {
                // Need to Update PricePoints
                URLFetcher Fetcher = new URLFetcher(startURL + CardIn.URL);
                string ret = Fetcher.Fetch();

                retPP = _HTMLParser.ParsePricePoints(ret, CardIn);

                CardIn.LastPricePointUpdate = DateTime.Today;

                _SQLWrapper.UpdatePricePoints(retPP, CardIn);
                _SQLWrapper.UpdateCardLastUpdate(CardIn, CardIn.LastPricePointUpdate);
            }
            else
            {
                // PricePoints are already up to date, parse from SQL
                retPP = _SQLWrapper.GetPricePoints(CardIn);
            }

            retPP = retPP.OrderByDescending(pp => pp.Date).ToList();
            CurrentPricePoints = retPP;
            return retPP;
        }

        public List<PricePoint> ApplyFilters(List<PricePoint> PPIn, FilterTypes FilterTypeIn)
        {
             return _DataFilter.ApplyDataFilters(PPIn, FilterTypeIn);
        }

        /* Updating the application state to be stored */
        public void UpdateAppState(List<int> CheckedPriceSources, List<int> CheckedMTGSets, List<int> DataFilters)
        {
            _ApplicationState.UpdateAppState(CheckedPriceSources, CheckedMTGSets, DataFilters);
        }

        public void GetAppState(ref List<int> CheckedPriceSources, ref List<int> CheckedMTGSets, ref List<int> DataFilters)
        {
            _ApplicationState.GetAppState(ref CheckedPriceSources, ref CheckedMTGSets, ref DataFilters);
        }

        /* Simple getters for private member variables */
        public List<MTGSet> GetSets()
        {
            return Sets;
        }

        public MTGCard GetCurrentCard()
        {
            return CurrentCard;
        }

        public List<PricePoint> GetCurrentPricePoints()
        {
            return CurrentPricePoints;
        }
    }
}
