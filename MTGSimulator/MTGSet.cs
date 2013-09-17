using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGUtils
{
    public class MTGSet
    {
        string SetName;
        DateTime SetDate;
        DateTime SetLastUpdate;
        public string URL { get; set;}

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
