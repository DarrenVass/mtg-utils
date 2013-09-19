using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGUtils
{
    public class MTGSet
    {
        protected string SetName;
        public DateTime SetDate { get; set; }
        public DateTime SetLastUpdate { get; set; }
        public string URL { get; set;}
        public string FoilURL { get; set; }

        public MTGSet(string nameIn, DateTime dateIn, DateTime lastUpdateIn)
        {
            SetName = nameIn;
            SetDate = dateIn;
            SetLastUpdate = lastUpdateIn;
        }

        public override string ToString()
        {
            return SetName;
        }

        
    }
}
