using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGSimulator
{
    public class MTGSet
    {
        string SetName;
        DateTime SetDate;
        DateTime SetLastUpdate;

        public MTGSet(string NameIn, DateTime DateIn, DateTime LastUpdateIn)
        {
            SetName = NameIn;
            SetDate = DateIn;
            SetLastUpdate = LastUpdateIn;
        }

        public override string ToString()
        {
            return SetName;
        }
    }
}
