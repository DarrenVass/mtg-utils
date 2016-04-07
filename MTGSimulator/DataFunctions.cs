using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
