using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using log4net;


/*
 * Class for simplifying the storage of the state. Description of
 * specific variables to be in the code
 */

namespace MTGUtils
{
    [Serializable]
    public class WholeAppState
    {
        public List<int> PriceSourceState;    // List reflecting which source indices are checked
        public List<int> MTGSetsState;        // List reflecting which MTG set indicies are checked
        public List<int> DataFilterState;     // List reflecting which data filters are to be applied
    }

    public class AppState:IDisposable
    {
        private readonly ILog log;

        private string FileName = "ApplicationStoredState.mus";

        public WholeAppState AppData { get; set; }

        public AppState()
        {
            AppData = new WholeAppState();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            this.ParseAppState();
        }

        public void Dispose()
        {
            StoreAppState();
        }

        public void UpdateAppState(List<int> CheckedPriceSources, List<int> CheckedMTGSets, List<int> DataFilters)
        {
            AppData.PriceSourceState = CheckedPriceSources;
            AppData.MTGSetsState = CheckedMTGSets;
            AppData.DataFilterState = DataFilters;
        }

        public void GetAppState(ref List<int> CheckedPriceSources, ref List<int> CheckedMTGSets, ref List<int> DataFilters)
        {
            CheckedPriceSources = AppData.PriceSourceState;
            CheckedMTGSets = AppData.MTGSetsState;
            DataFilters = AppData.DataFilterState;
        }

        /* Get File state from the file */
        private void ParseAppState()
        {
            try
            {
                using (FileStream stream = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    
                    AppData = (WholeAppState)bin.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                log.Error("ParseAppState Error: " + e.Message);
            }

        }

        /* Store the application state in a file */
        private void StoreAppState()
        {
            try
            {
                using (FileStream stream = new FileStream(FileName, FileMode.Truncate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, AppData);
                }
            }
            catch (Exception e)
            {
                log.Error("StoreAppState Error: " + e.Message);
            }
        }
    }
}
