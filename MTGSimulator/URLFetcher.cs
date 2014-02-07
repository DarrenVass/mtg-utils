using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Web;
using System.IO;

using log4net;

namespace MTGUtils
{
    class URLFetcher
    {
        string URL;
        private readonly ILog log;

        public URLFetcher(string URLIn)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            URL = URLIn;
        }

        public string Fetch()
        {
            log.Debug("Fetching set URLs from: " + URL);
            WebRequest req = WebRequest.Create(URL);
            req.Method = "GET";
            WebResponse resp = req.GetResponse();
            StreamReader rdr = new StreamReader(resp.GetResponseStream());
            string content = rdr.ReadToEnd();
            rdr.Close();
            resp.Close();
            return content;
        }
    }
}
