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

            string createCardTable = "CREATE TABLE IF NOT EXISTS mtgCards(cardName varchar(256) NOT NULL PRIMARY KEY, setName varchar(256) NOT NULL," +
                                        "price int, url varchar(256), foilURL varchar(256), lastUpdate date);";
            SQLiteCommand cmd2 = new SQLiteCommand(createCardTable, MTGDB);
            cmd2.ExecuteNonQuery();
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
                        cmd.Parameters.Clear();
                        cmd.CommandText = "INSERT OR REPLACE INTO mtgSets (setName, urlList, foilURLList, lastUpdate, releaseDate) " +
                                            "VALUES (@NAME, @URL, @FURL, @LU, @RD)";
                        cmd.Parameters.AddWithValue("@URL", set.URL);
                        cmd.Parameters.AddWithValue("@FURL", set.FoilURL);
                        cmd.Parameters.AddWithValue("@LU", set.CardListLastUpdate);
                        cmd.Parameters.AddWithValue("@RD", set.SetDate);
                        cmd.Parameters.AddWithValue("@Name", set.ToString());
                        sum += cmd.ExecuteNonQuery();
                    }
                    trn.Commit();
                }
                catch (Exception err)
                {
                    log.Error("Insert/Update Error:", err);
                }
            }
        }

        /* Returns a List of the MTGCards from the DB for a given set */
        public List<MTGCard> GetCardList(string SetName)
        {
            List<MTGCard> retCards = new List<MTGCard>();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                {
                    cmd.CommandText = "SELECT * FROM mtgCards WHERE setName=@SNAME";
                    cmd.Parameters.AddWithValue("@SNAME", SetName);
                    SQLiteDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        UInt64 price = Convert.ToUInt64(rdr["price"]);
                        MTGCard card = new MTGCard(rdr["cardName"].ToString(), rdr["setName"].ToString(), price);
                        card.LastPricePointUpdate = (DateTime)rdr["lastUpdate"];
                        card.URL = rdr["url"].ToString();
                        card.FoilURL = rdr["foilURL"].ToString();
                        retCards.Add(card);
                    }
                }
            }
            catch (Exception err)
            {
                log.Warn("GetCardList() for set " +  SetName + " Err:", err);
            }
            return retCards;
        }

        /* For saving the updated card list for a given set */
        public void UpdateCardList(List<MTGCard> CardsIn, string SetName)
        {
            int sum = 0;
            foreach (MTGCard card in CardsIn)
            {
                try
                {
                    SQLiteTransaction trn = MTGDB.BeginTransaction();
                    using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "INSERT OR REPLACE INTO mtgCards (cardName, setName, price, url, foilURL, lastUpdate) " +
                                            "VALUES (@CNAME, @SNAME, @PRICE, @URL, @FURL, @LU)";
                        cmd.Parameters.AddWithValue("@URL", card.URL);
                        cmd.Parameters.AddWithValue("@FURL", card.URL);
                        cmd.Parameters.AddWithValue("@LU", card.LastPricePointUpdate);
                        cmd.Parameters.AddWithValue("@CName", card.CardName);
                        cmd.Parameters.AddWithValue("@SName", card.SetName);
                        cmd.Parameters.AddWithValue("@PRICE", card.Price);
                        sum += cmd.ExecuteNonQuery();
                    }
                    trn.Commit();
                }
                catch (Exception err)
                {
                    log.Error("Insert/Update Error:", err);
                }
            }
        }

        public void UpdateSetLastUpdate(string SetName, DateTime LastCardListUpdate)
        {
            SQLiteTransaction trn = MTGDB.BeginTransaction();
            using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
            {
                cmd.CommandText = "UPDATE mtgSets SET lastUpdate=@DATE where setname=@SNAME";
                cmd.Parameters.AddWithValue("@SNAME", SetName);
                cmd.Parameters.AddWithValue("@DATE", LastCardListUpdate);
                cmd.ExecuteNonQuery();
            }
            trn.Commit();
        }
    }
}
