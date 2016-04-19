using System;
using System.Collections.Generic;

namespace MTGUtils
{
    [Serializable]
    public class MTGFormat
    {
        public string FormatName { get; set; }
        public DateTime FormatListLastUpdate { get; set; }
        public List<string> Sets { get; set; }

        public MTGFormat(string formatNameIn)
        {
            this.FormatName = formatNameIn;
        }

        public override string ToString()
        {
            return this.FormatName;
        }

        
    }
}
