using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            CommonData.BackupDb();

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            database = DalFactory.CreateDatabase();
            playerDao = DalFactory.CreatePlayerDao(database);
            ratingDao = DalFactory.CreateRatingDao(database);

            testPlayer = playerDao.FindById(0);
            if (testPlayer == null)
            {
                testPlayer = new Player("first", "last", "nic2k", "us7er", "pass",
                    false, false, false, false, false, true, true, true, null);
                int playerId = playerDao.Insert(testPlayer);
            }
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
            int foundInitial = ratingDao.FindAll().Count;
            int cntInitial = ratingDao.Count();
            Assert.AreEqual(foundInitial, cntInitial);

            const int insertAmount = 10;
            
            for (var i = 0; i < insertAmount; ++i)
            {
                Rating rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
                ratingDao.Insert(rating);
            }
            int cntAfterInsert = ratingDao.Count();
            Assert.AreEqual(insertAmount + foundInitial, cntAfterInsert);

            int foundAfterInsert = ratingDao.FindAll().Count;
            Assert.AreEqual(cntAfterInsert, foundAfterInsert);
        }

        [TestMethod]
        public void FindAllByPlayer()
        {
            int foundInitial = ratingDao.FindAllByPlayer(RatingTests.testPlayer).Count;
            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                Rating rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
                ratingDao.Insert(rating);
            }

            int foundAfterInsert = ratingDao.FindAllByPlayer(RatingTests.testPlayer).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterInsert);
        }

        [TestMethod]
        public void Insert()
        {
            int cnt = ratingDao.Count();
            var rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
            var ratingId = ratingDao.Insert(rating);
            int newCnt = ratingDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsInstanceOfType(ratingId, typeof(int));
            Assert.IsTrue(ratingId >= 0);
            Assert.AreEqual(ratingId, rating.RatingId);
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

        [TestMethod]
        public void Update()
        {
            var rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);

            int ratingId = ratingDao.Insert(rating);

            var newValue = 3000;
            rating.Value = newValue;
            ratingDao.Update(rating);

            rating = ratingDao.FindById(ratingId);
            Assert.AreEqual(newValue, rating.Value);
        }

        [TestMethod]
        public void UpdateWithoutPlayerIdFails()
        {
            Player player = playerDao.FindById(0);
            if (player == null)
            {
                player = new Player("first", "last", "nic2k", "us7er", "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
                player.PlayerId = null;
            }
            try
            {
                ratingDao.Update(new Rating(player, new DateTime(2000, 1, 1), 2000)); // should throw ArgumentException
                Assert.Fail("ArgumentException not thrown for invalid Player.Update()");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void FindCurrentRating()
        {
            var rating = new Rating(testPlayer, DateTime.Now, 2345);
            ratingDao.Insert(rating);
            var mostRecentRating = ratingDao.FindCurrentRating(testPlayer);
            Assert.AreEqual(rating.RatingId, mostRecentRating.RatingId);
        }
    }
}
