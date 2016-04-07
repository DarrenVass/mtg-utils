using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace MTGUtils
{
    public struct FilterTypes
    {
        public List<string> RetailerList { get; set; }
        public bool StdDev { get; set; }
        public bool Future { get; set; }
        public bool Average { get; set; }
    }

    /*
     * Class for receiving raw unfiltered data which will then apply filters to change output data
     *  - Non-zero filter                   - Remove all instances of $0.00
     *  - Within one std deviation filter   - To remove outlying data
     *  - Average filter                    - To add a set of data titled 'Average
     *  - Future predictions                - To extend the data points to 3 months past current date
     */
    public class DataFilter
    {
        private readonly ILog log;
        private string MTGUtilsAverage; // 

        public DataFilter()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            MTGUtilsAverage = "Average"; // The retailer for the Average function.
        }

        /*
         * Input: Sorted by data list of PricePoints
         *          - Boolean values denoting which filters to apply
         * Output: Sorted by data list of PricePoints after selected filters have been applied.
         * Info: The order the filters are applied matters as removing invalid data should happen
         *        before doing calculations based on the data.
         *        - Also must copy the list otherwise it will apply to the DataIn
         */
        public List<PricePoint> ApplyDataFilters(List<PricePoint> DataIn, FilterTypes Filters)
        {
            List<PricePoint> DataOut = new List<PricePoint>(DataIn);

            // Always Filter the retailers.
            if (Filters.StdDev)
                StdDevFilter(ref DataOut);
            if (Filters.Future)
                AddFutureFilter(ref DataOut);
            if (Filters.Average)
                AddAverageFilter(ref DataOut);

            return DataOut;
        }

        private UInt64 CalculateStdDev(List<PricePoint> DataIn, ref UInt64 AvgOut)
        {
            if (DataIn.Count < 2) return 0;
            double sumOfSquares = 0.0;
            UInt64 sum = 0;
            UInt16 count = 0;
            foreach (PricePoint pp in DataIn)
            {
                sum += pp.Price;
                count++;
            }
            double avg = sum / count;
            foreach (PricePoint pp in DataIn)
            {
                sumOfSquares += Math.Pow((pp.Price - avg), 2);
            }
            AvgOut = (UInt64)avg;
            return (UInt64)Math.Sqrt(sumOfSquares / (DataIn.Count - 1));
        }

        /*
         * Remove all price points that fall outside of a std deviation at a given date.
         */
        private List<PricePoint> StdDevFilter(ref List<PricePoint> DataIn)
        {
            DateTime curDate = DateTime.MinValue;
            List<PricePoint> pointsToRemove = new List<PricePoint>();
            List<PricePoint> listToGetSTDDEV = new List<PricePoint>();
            foreach (PricePoint pp in DataIn)
            {
                if (curDate == DateTime.MinValue)
                {
                    // New Date to check for STDDEV
                    curDate = pp.Date;
                    listToGetSTDDEV.Add(pp);
                }
                else if (curDate == pp.Date)
                {
                    // Another PP for current STDDEV
                    listToGetSTDDEV.Add(pp);
                }
                else
                {
                    // New Date, so get STDDEV and remember which PricePoints to remove
                    UInt64 avg = 0;
                    UInt64 stdDev = CalculateStdDev(listToGetSTDDEV, ref avg);
                    UInt64 min = (stdDev > avg) ? 0 : ( avg - stdDev );
                    UInt64 max = avg + stdDev;

                    // Add to list to be removed if outside <min, max>
                    foreach (PricePoint pp2 in listToGetSTDDEV)
                    {
                        if (pp2.Price < min || pp2.Price > max)
                        {
                            pointsToRemove.Add(pp2);
                        }
                    }

                    // Reset curDate
                    listToGetSTDDEV.Clear();
                    listToGetSTDDEV.Add(pp);
                    curDate = DateTime.MinValue;
                }
            }

            foreach (PricePoint pp in pointsToRemove)
            {
                DataIn.Remove(pp);
            }

            return DataIn;
        }

        /*
         * Extend current price point patterns per retailer for 3 months past last point.
         */
        private List<PricePoint> AddFutureFilter(ref List<PricePoint> DataIn)
        {
            return DataIn;
        }

        /*
         * Add an average point for each unique datetime.
         */
        private List<PricePoint> AddAverageFilter(ref List<PricePoint> DataIn)
        {
            DateTime curDate = DateTime.MinValue;
            UInt64 sum = 0;
            UInt64 count = 0;
            List<PricePoint> listToAdd = new List<PricePoint>();
            foreach (PricePoint pp in DataIn)
            {
                if (curDate == DateTime.MinValue)
                {
                    // New day to get average for
                    curDate = pp.Date;
                    sum += pp.Price;
                    count++;
                }
                else if (curDate.Day == pp.Date.Day && curDate.Month == pp.Date.Month && curDate.Year == pp.Date.Year)
                {
                    // Another PP for current day
                    sum += pp.Price;
                    count++;
                }
                else
                {
                    // New day, so get average and insert it at appropriate place.
                    PricePoint newAvg = new PricePoint();
                    newAvg.Date = curDate;
                    newAvg.Price = (UInt64)(sum / count);
                    newAvg.Retailer = MTGUtilsAverage;

                    listToAdd.Add(newAvg);

                    curDate = pp.Date;
                    sum = pp.Price;
                    count = 1;
                }
            }

            foreach (PricePoint pp in listToAdd)
            {
                DataIn.Add(pp);
            }

            DataIn = DataIn.OrderByDescending(pp => pp.Date).ToList();

            return DataIn;
        }

        
    }
}
