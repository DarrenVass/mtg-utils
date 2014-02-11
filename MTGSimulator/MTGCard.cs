using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGUtils
{
    struct PricePoint
    {
        public UInt64 Price { get; set; }
        public DateTime Date { get; set; }
    }

    public class MTGCard
    {
        public string CardName { get; set; }
        public string SetName { get; set; }
        public UInt64 Price { get; set; }
        public string URL { get; set; }
        public DateTime LastPricePointUpdate { get; set; }

        public MTGCard(string nameIn, UInt64 priceIn, string urlIn)
        {
            CardName = nameIn;
            Price = priceIn;
            URL = urlIn;
        }

        public override string ToString()
        {
            return CardName;
        }
    }
}
