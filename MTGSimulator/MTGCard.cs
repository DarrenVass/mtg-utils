using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGSimulator
{
    class MTGCard
    {
        string CardName;

        public MTGCard(string NameIn)
        {
            CardName = NameIn;
        }

        public override string ToString()
        {
            return CardName;
        }
    }
}
