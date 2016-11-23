using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Domain;

namespace WuHu.Test
{
    [TestClass]
    public class RatingTests
    {

        private static string connectionString;
        private static IDatabase database;
        private static IPlayerDao playerDao;
        private static IRatingDao ratingDao;
        private static Player testPlayer;
        public static TestContext PlayerTestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            database = DalFactory.CreateDatabase();
            CommonData.DropTables(database);
            CommonData.CreateTables(database);
            PlayerTestContext = testContext;
            playerDao = DalFactory.CreatePlayerDao(database);
            ratingDao = DalFactory.CreateRatingDao(database);
            
            testPlayer = new Player("first", "last", "nick", "user", "pass",
                false, false, false, false, false, true, true, true, null);
            int playerId = playerDao.Insert(testPlayer);
            testPlayer.PlayerId = playerId;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            CommonData.DropTables(database);
            CommonData.CreateTables(database);
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CommonData.DeleteAllFromTable(database, "Rating");
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(ratingDao);

            Rating rating = new Rating(0, testPlayer, new DateTime(2000, 1, 1), 2000);
            Assert.IsNotNull(rating);

            rating = new Rating(testPlayer, new DateTime(2000, 1, 1), 2000);
            Assert.IsNotNull(rating);
        }

        [TestMethod]
        public void Count()
        {
            int cnt1 = ratingDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            ratingDao.Insert(new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000));

            int cnt2 = ratingDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }
        
        [TestMethod]
        public void FindAll()
        {
            int cntInital = ratingDao.Count();
            const int insertAmount = 10;
            
            for (var i = 0; i < insertAmount; ++i)
            {
                Rating rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
                ratingDao.Insert(rating);
            }
            int cntAfterInsert = ratingDao.Count();
            Assert.AreEqual(insertAmount + cntInital, cntAfterInsert);

            IList<Rating> ratings = ratingDao.FindAll();
            Assert.AreEqual(ratings.Count + cntInital, cntAfterInsert);
        }

        [TestMethod]
        public void Insert()
        {
            int cnt = ratingDao.Count();
            var ratingId = ratingDao.Insert(new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000));
            int newCnt = ratingDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsInstanceOfType(ratingId, typeof(int));
            Assert.IsTrue(ratingId >= 0);
        }

        [TestMethod]
        public void InsertWithoutPlayerIdFails()
        {
            var player = new Player("", "", "", "", "", false, false,
                                false, false, false, false, false, false, null);

            try
            {
                ratingDao.Insert(new Rating(player, new DateTime(2000, 1, 1), 2000));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            { }
        }
    }
}
