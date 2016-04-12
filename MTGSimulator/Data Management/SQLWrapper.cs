﻿using System;
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
            try
            {
                MTGDB.Close();
            }
            catch (System.ObjectDisposedException e)
            {
                log.Error("Closed before finished using MTGDB: " + e);
            }
        }

        /* Will create the Set & Card tables if required */
        private void CreateTables()
        {
            string createSetTable = "CREATE TABLE IF NOT EXISTS mtgSets(setName varchar(256) NOT NULL PRIMARY KEY, urlList varchar(256)," +
                                        "foilURLList varchar(256), lastUpdate date, releaseDate date);";
            SQLiteCommand cmd1 = new SQLiteCommand(createSetTable, MTGDB);
            cmd1.ExecuteNonQuery();

            string createCardTable = "CREATE TABLE IF NOT EXISTS mtgCards(cardName varchar(256) NOT NULL PRIMARY KEY, setName varchar(256) NOT NULL," +
                                        "price int, url varchar(256), foilURL varchar(256), imageURL varchar(256), lastUpdate date);";
            SQLiteCommand cmd2 = new SQLiteCommand(createCardTable, MTGDB);
            cmd2.ExecuteNonQuery();

            string createPricePointTable = "CREATE TABLE IF NOT EXISTS mtgPP(cardName varchar(256) NOT NULL, setName varchar(256) NOT NULL," +
                                        "price int, retailer varchar(32) ,priceDate date);";
            SQLiteCommand cmd3 = new SQLiteCommand(createPricePointTable, MTGDB);
            cmd3.ExecuteNonQuery();
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
            try
            {
                SQLiteTransaction trn = MTGDB.BeginTransaction();
                foreach (MTGSet set in SetsIn)
                {
                
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
                }
                trn.Commit();
            }
            catch (Exception err)
            {
                log.Error("Insert/Update Error:", err);
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
                        card.CardImageURL = rdr["imageURL"].ToString();
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
            try
            {
                SQLiteTransaction trn = MTGDB.BeginTransaction();
                foreach (MTGCard card in CardsIn)
                {
                
                    using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "INSERT OR REPLACE INTO mtgCards (cardName, setName, price, url, foilURL, imageURL, lastUpdate) " +
                                            "VALUES (@CNAME, @SNAME, @PRICE, @URL, @FURL, @IURL, @LU)";
                        cmd.Parameters.AddWithValue("@URL", card.URL);
                        cmd.Parameters.AddWithValue("@FURL", card.FoilURL);
                        cmd.Parameters.AddWithValue("@IURL", card.CardImageURL);
                        cmd.Parameters.AddWithValue("@LU", card.LastPricePointUpdate);
                        cmd.Parameters.AddWithValue("@CName", card.CardName);
                        cmd.Parameters.AddWithValue("@SName", card.SetName);
                        cmd.Parameters.AddWithValue("@PRICE", card.Price);
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

        /* Returns a List of the PricePoints from the DB for a given Card for certain retailers retailers.*/
        public List<PricePoint> GetPricePoints(MTGCard CardIn, List<string> RetailerList)
        {
            if (CardIn == null)
            {
                log.Error("SQLWrapper::GetPricePoints was given a null CardIn.");
                return null;
            }

            List<PricePoint> retPP = new List<PricePoint>();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                {
                    cmd.CommandText = "SELECT * FROM mtgPP WHERE cardName=@CNAME AND setName=@SNAME";
                    cmd.Parameters.AddWithValue("@SNAME", CardIn.SetName);
                    cmd.Parameters.AddWithValue("@CNAME", CardIn.CardName);
                    // If no retailers it will just select all, otherwise specify retailers.
                    if (RetailerList.Count > 0)
                    {
                        cmd.CommandText += " AND (";
                        for (int i = 0; i < RetailerList.Count; i++)
                        {
                            cmd.CommandText += "retailer = @RNAME" + i.ToString();
                            if(i < RetailerList.Count() - 1) { cmd.CommandText += " OR "; } // Add OR for all statements except last one.
                            cmd.Parameters.AddWithValue("@RNAME" + i.ToString(), RetailerList[i]);
                        }
                        cmd.CommandText += ")";
                    }
                    cmd.CommandText += " ORDER BY priceDate DESC";
                    SQLiteDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PricePoint PP = new PricePoint();
                        PP.Date = (DateTime)rdr["priceDate"];
                        PP.Price = Convert.ToUInt64(rdr["price"]);
                        PP.Retailer = rdr["retailer"].ToString();
                        retPP.Add(PP);
                    }
                }
            }
            catch (Exception err)
            {
                log.Warn("GetPricePoints() for card " + CardIn.CardName + " in set " + CardIn.SetName + " Err:", err);
            }

            return retPP;
        }

        /* For saving the updated card list for a given set */
        public void UpdatePricePoints(List<PricePoint> PPsIn, MTGCard CardIn)
        {
            int sum = 0;
            try
            {
                SQLiteTransaction trn = MTGDB.BeginTransaction();
                foreach (PricePoint pp in PPsIn)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "INSERT OR REPLACE INTO mtgPP (cardName, setName, price, retailer, priceDate) " +
                                            "VALUES (@CNAME, @SNAME, @PRICE, @RET, @PDATE)";
                        cmd.Parameters.AddWithValue("@CNAME", CardIn.CardName);
                        cmd.Parameters.AddWithValue("@SNAME", CardIn.SetName);
                        cmd.Parameters.AddWithValue("@PRICE", pp.Price);
                        cmd.Parameters.AddWithValue("@RET", pp.Retailer);
                        cmd.Parameters.AddWithValue("@PDATE", pp.Date);
                        sum += cmd.ExecuteNonQuery();
                    }
                }
                trn.Commit();
            }
            catch(Exception err)
            {
                log.Error("Insert/Update Error: ", err);
            }
        }

        public void UpdateSetLastUpdate(string SetName, DateTime LastCardListUpdate)
        {
            try
            {
                SQLiteTransaction trn = MTGDB.BeginTransaction();
                using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                {
                    cmd.CommandText = "UPDATE mtgSets SET lastUpdate=@DATE WHERE setname=@SNAME";
                    cmd.Parameters.AddWithValue("@SNAME", SetName);
                    cmd.Parameters.AddWithValue("@DATE", LastCardListUpdate);
                    cmd.ExecuteNonQuery();
                }
                trn.Commit();
            }
            catch (Exception err)
            {
                log.Error("UpdateSetLastUpdate error: ", err);
            }
        }

        public void UpdateCardLastUpdate(MTGCard CardIn, DateTime LastPPUpdate)
        {
            try
            {
                SQLiteTransaction trn = MTGDB.BeginTransaction();
                using (SQLiteCommand cmd = new SQLiteCommand(MTGDB))
                {
                    cmd.CommandText = "UPDATE mtgCards SET lastUpdate=@DATE, imageURL=@IURL WHERE setname=@SNAME AND cardname=@CNAME";
                    cmd.Parameters.AddWithValue("@SNAME", CardIn.SetName);
                    cmd.Parameters.AddWithValue("@CNAME", CardIn.CardName);
                    cmd.Parameters.AddWithValue("@DATE", LastPPUpdate);
                    cmd.Parameters.AddWithValue("@IURL", CardIn.CardImageURL);
                    cmd.ExecuteNonQuery();
                }
                trn.Commit();
            }
            catch (Exception err)
            {
                log.Error("UpdateCardLastUpdate error: ", err);
            }
}
    }
}
