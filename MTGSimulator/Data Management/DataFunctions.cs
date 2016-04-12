using System;
using System.Collections.Generic;

using log4net;

namespace MTGUtils
{
    public class DataFunctions
    {
        private readonly ILog log;

        public DataFunctions()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /* Return the maximum and non-zero minimum prices.  */
        public void GetMinMax(List<PricePoint> PPsIn, ref UInt64 Min, ref UInt64 Max)
        {
            Min = UInt64.MaxValue;
            Max = UInt64.MinValue;
            foreach (PricePoint pp in PPsIn)
            {
                if (pp.Price != 0 && Min > pp.Price)
                    Min = pp.Price;
                if (Max < pp.Price)
                    Max = pp.Price;
            }
        }

        // For converting 123 in UInt64 to string "1.23" format
        public string GetPriceFromUInt64(UInt64 PriceIn)
        {
            return ((double)PriceIn/100).ToString("0.00");
        }

        /* Return the whole/3/7/30 day averages for the given pricepoints. */
        public void CalculateAverages(List<PricePoint> PPIn, ref UInt64 Avg, ref UInt64 Avg3Day, ref UInt64 Avg7Day, ref UInt64 Avg30Day)
        {
            UInt64 AvgCount = 0, Avg3Count = 0, Avg7Count = 0, Avg30Count = 0;
            foreach (PricePoint pp in PPIn)
            {
                Avg += pp.Price;
                AvgCount++;

                if ((DateTime.Now - pp.Date).TotalDays <= 3)
                {
                    Avg3Day += pp.Price;
                    Avg3Count++;
                }

                if ((DateTime.Now - pp.Date).TotalDays <= 7)
                {
                    Avg7Day += pp.Price;
                    Avg7Count++;
                }

                if ((DateTime.Now - pp.Date).TotalDays <= 30)
                {
                    Avg30Day += pp.Price;
                    Avg30Count++;
                }

            }
            Avg /= AvgCount;
            Avg3Day /= Avg3Count;
            Avg7Day /= Avg7Count;
            Avg30Day /= Avg30Count;
        }

    }
}
