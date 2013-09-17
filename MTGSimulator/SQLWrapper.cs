using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;
using log4net;

namespace MTGUtils
{
    class SQLWrapper
    {
        SQLiteConnection MTGDB;
        private readonly ILog log;

        public SQLWrapper()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                MTGDB = new SQLiteConnection("Data Source=MTGPriceDataBase.sqlite;Version=3;");
                MTGDB.Open();
                log.Debug("Using already created MTGPriceDataBase.sqlite");

                CreateTables();
            }
            catch(Exception err)
            {
                log.Fatal("Failed to Create SQLiteConnection",err);
            }
        }

        ~SQLWrapper()
        {
            MTGDB.Close();
        }

        /* Will create the Set & Card tables if required */
        private void CreateTables()
        {
            log.Debug("Creating Tables if required");
            string createSetTable = "create table if not exists sets(name varchar(50), urlList varchar(256), lastUpdate Date, releaseDate Date);";
            SQLiteCommand cmd1 = new SQLiteCommand(createSetTable, MTGDB);
            cmd1.ExecuteNonQuery();

            //string createCardTable = "create table cards(name varchar(100), url varchar(256), foilURL varchar(256), ";
            //SQLiteCommand cmd2 = new SQLiteCommand(createCardTable, mtgDB);
            //cmd2.ExecuteNonQuery();
        }

        /* Returns a List of the MTGSets from the DB */
        public List<MTGSet> GetSetList()
        {
            List<MTGSet> retSet = new List<MTGSet>();
            string getSetList = "select * from sets";
            SQLiteCommand cmd = new SQLiteCommand(getSetList, MTGDB);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //MTGSet set = new MTGSet(rdr["name"], rdr["releaseDate"], rdr["lastUpdate"])); // TODO SQLDate -> DateTime
                //set.URL = rdr["url"];
                //retSet.Add(set);
            }
            return retSet;
        }

        /* For saving the updated set list */
        public void UpdateSetList(List<MTGSet> SetsIn)
        {

        }
    }
}
