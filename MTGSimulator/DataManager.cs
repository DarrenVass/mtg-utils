using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MTGSimulator
{
    class DataManager
    {
        List<MTGSet> Sets;
        SQLWrapper sqlWrapper;

        public DataManager()
        {
            sqlWrapper = new SQLWrapper();

            Sets = sqlWrapper.GetSetList();
        }

        public void UpdateSets()
        {

        }

        public List<MTGSet> GetSets()
        {
            return Sets;
        }
    }
}
