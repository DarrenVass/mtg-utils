/*
 * This file is specifically for scrapping HTML on mtgprice.com.
 * It is not always going to be pretty, but since they can change their layout at any moment and this is a personal project
 *  I'm not going to make it super rigid as I don't have control over their data. 
 *  I've tried to get API access at http://www.mtgprice.com/mtgPriceAPI.jsp but they've never responded.
 */

using System;
using System.Collections.Generic;
using System.Linq;

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
        private MTGFormat CurrentFormat;

        private DateTime LastSetsUpdate { get; set; }

        // For accessing/storing of card data
        private string mainURL = "http://www.mtgprice.com/magic-the-gathering-prices.jsp";
        private string startURL = "http://www.mtgprice.com";
        private SQLWrapper _SQLWrapper;
        private MTGPriceParser _MTGPriceParser;
        private FormatParser _FormatParser;
        private DataFilter _DataFilter;

        // For saving/loading of application state data
        private MTGUtils.AppState _ApplicationState;

        public DataManager()
        {
            _SQLWrapper = new SQLWrapper();
            _MTGPriceParser = new MTGPriceParser();
            _FormatParser = new FormatParser();
            _DataFilter = new DataFilter();
            _ApplicationState = new MTGUtils.AppState();

            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Sets = _SQLWrapper.GetSetList();
            CurrentCard = null;
            CurrentPricePoints = null;
            CurrentFormat = null;
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

            Sets = _MTGPriceParser.ParseSets(ret);

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

                curCards = _MTGPriceParser.ParseCardURLs(ret, SetIn.ToString());
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

        /* Price Point fetching/parsing for a particular card/set and list of retailers*/
        public List<PricePoint> GetPricePointsForCard(MTGCard CardIn, List<string> RetailerList)
        {
            if (CardIn == null)
            {
                log.Error("UpdatePricePoints supplied null MTGCard");
                return null;
            }
            CurrentCard = CardIn;

            // Need to Update PricePoints
            if (CardIn.LastPricePointUpdate.CompareTo(DateTime.Today) < 0)
            {
                List<PricePoint> parsePP = new List<PricePoint>();
                URLFetcher Fetcher = new URLFetcher(startURL + CardIn.URL);
                string ret = Fetcher.Fetch();

                parsePP = _MTGPriceParser.ParsePricePoints(ret, CardIn);

                CardIn.LastPricePointUpdate = DateTime.Today;

                _SQLWrapper.UpdatePricePoints(parsePP, CardIn);
                _SQLWrapper.UpdateCardLastUpdate(CardIn, CardIn.LastPricePointUpdate);
            }

            // Select from SQL for given retailers.
            List<PricePoint> retPP = _SQLWrapper.GetPricePoints(CardIn, RetailerList);

            CurrentPricePoints = retPP;
            return retPP;
        }

        /* Parse all cards for the given list of sets and store properly. */
        public void ParseAllCards(List<MTGSet> SetsIn)
        {
            if (SetsIn == null || SetsIn.Count == 0)
            {
                log.Error("ParseAllCards() SetsIn was empty.");
                return;
            }
            foreach (MTGSet set in SetsIn)
            {
                if (set.CardListLastUpdate.CompareTo(DateTime.Today) < 0)
                {
                    List<MTGCard> curCards = new List<MTGCard>();

                    // Need to Update List
                    URLFetcher Fetcher = new URLFetcher(startURL + set.URL);
                    string ret = Fetcher.Fetch();

                    curCards = _MTGPriceParser.ParseCardURLs(ret, set.ToString());
                    curCards = curCards.OrderBy(card => card.ToString()).ToList();

                    set.CardListLastUpdate = DateTime.Today;

                    _SQLWrapper.UpdateCardList(curCards, set.ToString());
                    _SQLWrapper.UpdateSetLastUpdate(set.ToString(), set.CardListLastUpdate);
                }
            }
        }

        /* Parse all price points for the given set and store properly. Ignore any cards that are up to date or under the given price.*/
        public void ParseAllPricePoints(MTGSet SetIn, UInt64 PriceIn)
        {
            
            if (SetIn == null)
            {
                log.Error("ParseAllPricePoints() SetIn was null.");
                return;
            }
            if (SetIn.Cards == null || SetIn.Cards.Count == 0)
            {
                log.Error("ParseAllPricePoints() SetIn had no cards.");
                return;
            }

            foreach(MTGCard card in SetIn.Cards)
            {
                if ((card.LastPricePointUpdate.CompareTo(DateTime.Today) > 0) || 
                        (card.Price < PriceIn))
                { continue; }

                List<PricePoint> parsePP = new List<PricePoint>();
                URLFetcher Fetcher = new URLFetcher(startURL + card.URL);
                string ret = Fetcher.Fetch();

                parsePP = _MTGPriceParser.ParsePricePoints(ret, card);

                card.LastPricePointUpdate = DateTime.Today;

                _SQLWrapper.UpdatePricePoints(parsePP, card);
                _SQLWrapper.UpdateCardLastUpdate(card, card.LastPricePointUpdate);
            }
        }

        /* From a list of set name strings get the matching MTGSets. */
        private List<MTGSet> ConvertStringsToSets(List<string> StringsIn)
        {
            log.Error("ConvertStringsToSets");
            if (StringsIn == null) { return null; }

            List<MTGSet> ret = new List<MTGSet>();

            foreach(string setName in StringsIn)
            {
                try
                {
                    MTGSet z = Sets.Single(p => p.SetName.Equals(setName, StringComparison.OrdinalIgnoreCase));
                    ret.Add(z);
                }
                catch(System.InvalidOperationException)
                {
                    log.Error("ConvertStringsToSets() failed for set name '" + setName + "'");
                }
            }           

            return ret;
        }
        
        /* Get Stanard format. May need to parse from HTML or just from the appstate. */
        public List<MTGSet> RetrieveStandardFormat()
        {
            _ApplicationState.GetStandardFormat(ref CurrentFormat);

            if (CurrentFormat == null || CurrentFormat.FormatListLastUpdate.CompareTo(DateTime.Today) < 0)
            {
                // Need to update the format.
                URLFetcher Fetcher = new URLFetcher(_FormatParser.StandardURL);
                string ret = Fetcher.Fetch();
                CurrentFormat = _FormatParser.ParseFormat(ret, _FormatParser.StandardFormatName);
                if (CurrentFormat == null) { return null; }
            }

            _ApplicationState.UpdateStandardFormat(CurrentFormat);
            return ConvertStringsToSets(CurrentFormat.Sets);
        }

        /* Get Modern format. May need to parse from HTML or just from the appstate. */
        public List<MTGSet> RetrieveModernFormat()
        {
            _ApplicationState.GetModernFormat(ref CurrentFormat);

            if (CurrentFormat == null || CurrentFormat.FormatListLastUpdate.CompareTo(DateTime.Today) < 0)
            {
                // Need to update the format.
                URLFetcher Fetcher = new URLFetcher(_FormatParser.ModernURL);
                string ret = Fetcher.Fetch();
                CurrentFormat = _FormatParser.ParseFormat(ret, _FormatParser.ModernFormatName);
                if (CurrentFormat == null) { return null; }
            }

            _ApplicationState.UpdateModernFormat(CurrentFormat);
            return ConvertStringsToSets(CurrentFormat.Sets);
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

        /* Applying Data Filters */
        public List<PricePoint> ApplyFilters(List<PricePoint> PPIn, FilterTypes FilterTypeIn)
        {
            return _DataFilter.ApplyDataFilters(PPIn, FilterTypeIn);
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
