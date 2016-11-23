using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.Dal.Common;
using WuHu.Domain;
using System.Data.SqlClient;

namespace WuHu.Test
{
    [TestClass]
    public class PlayerTests
    {
        private static string connectionString;
        private static IDatabase database;
        private static IPlayerDao playerDao;
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
            CommonData.DeleteAllFromTable(database, "Player");
        }

        private string GenerateNickName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20);
        }
        
        [TestMethod]
        public void Insert()
        {
            //generates a random string for our user field
            string uniqueUsername = GenerateNickName();
            int cnt = playerDao.Count();
            var playerId = playerDao.Insert(new Player("first", "last", "nick", uniqueUsername, "pass", 
                false, false, false, false, false, true, true, true, null));
            int newCnt = playerDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsInstanceOfType(playerId, typeof(int));
            Assert.IsTrue(playerId >= 0);
        }

        [TestMethod]
        public void InsertUserUniqueConstraint()
        {
            string uniqueUsername = GenerateNickName();
            var player = new Player("", "", "", uniqueUsername, "", false, false, 
                                false, false, false, false, false, false, null);

            playerDao.Insert(player);
            try
            {
                playerDao.Insert(player);
                Assert.Fail("No SqlException thrown.");
            }
            catch (SqlException)
            { }
        }

        [TestMethod]
        public void Count()
        {
            int cnt1 = playerDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            string uniqueUsername = GenerateNickName();
            playerDao.Insert(new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null));

            int cnt2 = playerDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void Update()
        {
            string uniqueUsername = GenerateNickName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);

            int playerId = playerDao.Insert(player);

            string newFirst = "newFirst";
            player.PlayerId = playerId;
            player.FirstName = newFirst;
            playerDao.Update(player);

            player = playerDao.FindById(playerId);
            Assert.AreEqual(newFirst, player.FirstName);
        }

        [TestMethod]
        public void UpdateWithoutPlayerIdFails()
        {
            string uniqueUsername = GenerateNickName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            try
            {
                playerDao.Update(player); // should throw ArgumentException
                Assert.Fail("ArgumentException not thorwn for invalid Player.Update()");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void Constructor()
        {

            string uniqueUsername = GenerateNickName();
            var player1 = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player1.UserName, uniqueUsername);

            uniqueUsername = GenerateNickName();
            var pw = new byte[32];
            var salt = new byte[32];
            var player2 = new Player(0, "first", "last", "nick", uniqueUsername, pw, salt,
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player2.UserName, uniqueUsername);
        }

        [TestMethod]
        public void FindAll()
        {
            int cntInital = playerDao.Count();

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateNickName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }

            int cntAfterInsert = playerDao.Count();
            Assert.AreEqual(insertAmount + cntInital, cntAfterInsert);

            var players = playerDao.FindAll();
            Assert.AreEqual(players.Count + cntInital, cntAfterInsert);
        }

        [TestMethod]
        public void FindAllByString()
        {
            int cntInital = playerDao.Count();

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateNickName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }


            int cntAfterFirstInsert = playerDao.Count();
            Assert.AreEqual(insertAmount + cntInital, cntAfterFirstInsert);

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateNickName();
                Player player = new Player("other", "other", "other", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }

            int cntAfterSecondInsert = playerDao.Count();
            Assert.AreEqual(insertAmount * 2 + cntInital, cntAfterSecondInsert);

            var players = playerDao.FindAllByString("last");
            Assert.AreEqual(insertAmount, players.Count);
        }

        [TestMethod]
        public void FindAllByDay()
        {
            int cntInital = playerDao.Count();

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateNickName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, false, false, false, null);
                playerDao.Insert(player);
            }


            int cntAfterFirstInsert = playerDao.Count();
            Assert.AreEqual(insertAmount + cntInital, cntAfterFirstInsert);

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateNickName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, true, true, true, true, true, true, true, null);
                playerDao.Insert(player);
            }

            int cntAfterSecondInsert = playerDao.Count();
            Assert.AreEqual(insertAmount * 2 + cntInital, cntAfterSecondInsert);

            var players = playerDao.FindAllOnDays(monday: true);
            Assert.AreEqual(insertAmount, players.Count);
        }
    }
    
}
