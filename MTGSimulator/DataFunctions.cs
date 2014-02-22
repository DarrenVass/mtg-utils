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

        public void GetMinMax(List<PricePoint> PPsIn, ref UInt64 Min, ref UInt64 Max)
        {
            Min = UInt64.MaxValue;
            Max = UInt64.MinValue;
            foreach (PricePoint pp in PPsIn)
            {
                if (Min > pp.Price)
                    Min = pp.Price;
                if (Max < pp.Price)
                    Max = pp.Price;
            }
        }

        // For converting 123 in UInt64 to string "1.23" format
        public string GetPriceFromUInt64(UInt64 PriceIn)
        {
            return (PriceIn/100).ToString("0.00");
        }

    }
}
