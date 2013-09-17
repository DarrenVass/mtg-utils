using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGUtils
{
    class MTGCard
    {
        string CardName;

        public MTGCard(string nameIn)
        {
            CardName = nameIn;
        }

        public override string ToString()
        {
            return CardName;
        }
    }
}
