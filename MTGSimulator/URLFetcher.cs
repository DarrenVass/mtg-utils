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
            log.Info("Fetching URL : " + URL);
            WebRequest req = WebRequest.Create(URL);
            req.Method = "GET";
            WebResponse resp = req.GetResponse();
            StreamReader rdr = new StreamReader(resp.GetResponseStream());
            string content = rdr.ReadToEnd();
            rdr.Close();
            resp.Close();
            return content;
        }

        public void FetchImage(string StoragePath)
        {
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(this.URL);

            // returned values are returned as a stream, then read into a string
            String lsResponse = string.Empty;
            using (HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    using (FileStream lxFS = new FileStream(StoragePath, FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
                    }
                }
            }
        }
    }
}
