using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;
using log4net;

namespace MTGSimulator
{
    class SQLWrapper
    {
        SQLiteConnection mtgDB;
        private readonly ILog log;

        public SQLWrapper()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                mtgDB = new SQLiteConnection("Data Source=MTGPriceDataBase.sqlite;Version=3;");
                mtgDB.Open();
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
            mtgDB.Close();
        }

        /* Will create the Set & Card tables if required */
        private void CreateTables()
        {
            log.Debug("Creating Tables if required");
            string createSetTable = "create table if not exists sets(name varchar(50), urlList varchar(256), lastUpdate Date, releaseDate Date);";
            SQLiteCommand cmd1 = new SQLiteCommand(createSetTable, mtgDB);
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
            SQLiteCommand cmd = new SQLiteCommand(getSetList, mtgDB);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //retSet.Add(new MTGSet(rdr["name"], rdr["releaseDate"], rdr["lastUpdate"])); TODO SQLDate -> DateTime
            }
            return retSet;
        }
    }
}
