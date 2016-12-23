using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Domain;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class RatingTests
    {
        private static IDatabase database;
        private static IPlayerDao playerDao;
        private static IRatingDao ratingDao;
        private static Player testPlayer;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            database = DalFactory.CreateDatabase();
            playerDao = DalFactory.CreatePlayerDao(database);
            ratingDao = DalFactory.CreateRatingDao(database);

            testPlayer = playerDao.FindById(0);
            if (testPlayer == null)
            {
                testPlayer = new Player("first", "last", "nic2k", "us7er", "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(testPlayer);
            }
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(ratingDao);

            var rating = new Rating(0, testPlayer, new DateTime(2000, 1, 1), 2000);
            Assert.IsNotNull(rating);

            rating = new Rating(testPlayer, new DateTime(2000, 1, 1), 2000);
            Assert.IsNotNull(rating);
        }

        [TestMethod]
        public void Count()
        {
            var cnt1 = ratingDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            ratingDao.Insert(new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000));

            var cnt2 = ratingDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void FindById()
        {
            var rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
            ratingDao.Insert(rating);
            Assert.IsNotNull(rating.RatingId);
            var foundRating = ratingDao.FindById(rating.RatingId.Value);

            Assert.AreEqual(rating.RatingId, foundRating.RatingId);

            var nullRating = ratingDao.FindById(-1);
            Assert.IsNull(nullRating);
        }
 
        [TestMethod]
        public void FindAll()
        {
            var foundInitial = ratingDao.FindAll().Count;
            var cntInitial = ratingDao.Count();
            Assert.AreEqual(foundInitial, cntInitial);

            const int insertAmount = 10;
            
            for (var i = 0; i < insertAmount; ++i)
            {
                var rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
                ratingDao.Insert(rating);
            }
            var cntAfterInsert = ratingDao.Count();
            Assert.AreEqual(insertAmount + foundInitial, cntAfterInsert);

            var foundAfterInsert = ratingDao.FindAll().Count;
            Assert.AreEqual(cntAfterInsert, foundAfterInsert);
        }
        
        [TestMethod]
        public void FindAllByPlayer()
        {
            var foundInitial = ratingDao.FindAllByPlayer(RatingTests.testPlayer).Count;
            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                var rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
                ratingDao.Insert(rating);
            }

            var foundAfterInsert = ratingDao.FindAllByPlayer(RatingTests.testPlayer).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterInsert);

            try
            {
                var player = new Player("", "", "", "", "", false, false,
                                   false, false, false, false, false, false, null);
                ratingDao.FindAllByPlayer(player);
                Assert.Fail("No ArgumentException thrown");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void Insert()
        {
            var cnt = ratingDao.Count();
            var rating = new Rating(RatingTests.testPlayer, new DateTime(2000, 1, 1), 2000);
            ratingDao.Insert(rating);
            Assert.IsNotNull(rating.RatingId);
            var newCnt = ratingDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsTrue(rating.RatingId >= 0);
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

            ratingDao.Insert(rating);
            Assert.IsNotNull(rating.RatingId);

            var newValue = 3000;
            rating.Value = newValue;
            ratingDao.Update(rating);

            rating = ratingDao.FindById(rating.RatingId.Value);
            Assert.AreEqual(newValue, rating.Value);
        }

        [TestMethod]
        public void UpdateWithoutIdFails()
        {
            
            var player = new Player("first", "last", "nick", "user", "pass",
                    false, false, false, false, false, true, true, true, null);
            try // no playerId
            {
                ratingDao.Update(new Rating(0, player, new DateTime(2000, 1, 1), 2000)); 
                Assert.Fail("No ArgumentException thrown");
            }
            catch (ArgumentException)
            { }

            try // no ratingId
            {
                ratingDao.Update(new Rating(testPlayer, new DateTime(2000, 1, 1), 2000));
                Assert.Fail("No ArgumentException thrown");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void FindCurrentRating()
        {
            var rating = new Rating(testPlayer, DateTime.Now, 2345);
            ratingDao.Insert(rating);
            var mostRecentRating = ratingDao.FindCurrentRating(testPlayer);
            Assert.AreEqual(rating.RatingId, mostRecentRating.RatingId);

            var username = TestHelper.GenerateName();
            var playerWithoutId = new Player("", "", "", username, "", false, false,
                                false, false, false, false, false, false, null);

            try
            {
                ratingDao.FindCurrentRating(playerWithoutId);
                Assert.Fail("No ArgumentException thrown");
            }
            catch (ArgumentException) { }

            playerDao.Insert(playerWithoutId);
            var nullRating = ratingDao.FindCurrentRating(playerWithoutId);
            Assert.IsNull(nullRating);
        }
    }
}
