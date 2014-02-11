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
            string createSetTable = "CREATE TABLE IF NOT EXISTS mtgSets(setName varchar(256) NOT NULL PRIMARY KEY, urlList varchar(256)," +
                                        "foilURLList varchar(256), lastUpdate date, releaseDate date);";
            SQLiteCommand cmd1 = new SQLiteCommand(createSetTable, MTGDB);
            cmd1.ExecuteNonQuery();
        }

        /* Returns a List of the MTGSets from the DB */
        public List<MTGSet> GetSetList()
        {
            List<MTGSet> retSet = new List<MTGSet>();
            string getSetList = "SELECT * FROM mtgSets";
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(getSetList, MTGDB);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    DateTime rd = (DateTime)rdr["releaseDate"];
                    DateTime lu = (DateTime)rdr["lastUpdate"];

                    MTGSet set = new MTGSet(rdr["setName"].ToString(), rd);
                    set.CardListLastUpdate = lu;
                    set.URL = rdr["urlList"].ToString();
                    set.FoilURL = rdr["foilURLList"].ToString();
                    retSet.Add(set);
                }
            }
            catch (Exception err)
            {
                log.Warn("GetSetList():", err);
            }
            return retSet;
        }

        /* For saving the updated set list */
        public void UpdateSetList(List<MTGSet> SetsIn)
        {
            int sum = 0;
            foreach (MTGSet set in SetsIn)
            {
                try
                {
                    SQLiteTransaction trn = MTGDB.BeginTransaction();
                    using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                    {
                        cmd.CommandText = "SELECT * FROM mtgSets WHERE setName=@Name";
                        cmd.Parameters.AddWithValue("@Name", set.ToString());
                        if (cmd.ExecuteScalar() == null)
                        { // Does not Exist
                            cmd.Parameters.Clear();

                            cmd.CommandText = "INSERT INTO mtgSets (setName, urlList, foilURLList, lastUpdate, releaseDate) " +
                                                "VALUES (@NAME, @URL, @FURL, @LU, @RD)";
                            cmd.Parameters.AddWithValue("@URL", set.URL);
                            cmd.Parameters.AddWithValue("@FURL", set.FoilURL);
                            cmd.Parameters.AddWithValue("@LU", set.CardListLastUpdate);
                            cmd.Parameters.AddWithValue("@RD", set.SetDate);
                            cmd.Parameters.AddWithValue("@Name", set.ToString());
                            sum += cmd.ExecuteNonQuery();
                        }
                    }
                    trn.Commit();
                }
                catch (Exception err)
                {
                    log.Error("Insert/Update Error:", err);
                }
            }
        }
    }
}
