using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGUtils
{
    public struct PricePoint
    {
        public UInt64 Price { get; set; }
        public DateTime Date { get; set; }
        public string Retailer { get; set; }
    }

    public class MTGCard
    {
        public string CardName { get; set; }
        public string SetName { get; set; }
        public UInt64 Price { get; set; }
        public string URL { get; set; }
        public string FoilURL { get; set; }
        public DateTime LastPricePointUpdate { get; set; }

        public MTGCard(string cardNameIn, string setNameIn, UInt64 priceIn)
        {
            CardName = cardNameIn;
            SetName = setNameIn;
            Price = priceIn;
        }

        public override string ToString()
        {
            return CardName;
        }

        public string ToPriceString()
        {
            string name = CardName;
            name += " ($" + ((int)(Price / 100)).ToString() + "." + (Price % 100).ToString("00") +")";
            return name;
        }
    }
}
