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
        public void UpdateSets()
        {
            URLFetcher Fetcher = new URLFetcher(mainURL);
            string ret = Fetcher.Fetch();

            Sets = _HTMLParser.ParseSets(ret);
        }

        public List<MTGSet> GetSets()
        {
            return Sets;
        }
    }
}
