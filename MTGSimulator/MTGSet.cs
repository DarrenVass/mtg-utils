using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGUtils
{
    public class MTGSet
    {
        public string SetName { get; set; }
        public DateTime SetDate { get; set; }
        public DateTime CardListLastUpdate { get; set; }
        public string URL { get; set;}
        public string FoilURL { get; set; }
        public List<MTGCard> Cards { get; set; }

        public MTGSet(string nameIn, DateTime dateIn)
        {
            SetName = nameIn;
            SetDate = dateIn;
        }

        public override string ToString()
        {
            return SetName;
        }

        
    }
}
