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
        public string CardImageURL { get; set; }

        public MTGCard(string CardNameIn, string SetNameIn, UInt64 PriceIn)
        {
            this.CardName = CardNameIn;
            this.SetName = SetNameIn;
            this.Price = PriceIn;
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
