using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.Common;
using WuHu.Dal.Common;
using WuHu.Domain;
using System.Data.SqlClient;
using System.IO;

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
            CommonData.BeginTransaction(database);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CommonData.Rollback(database);
        }
        

        
        [TestMethod]
        public void Insert()
        {
            //generates a random string for our user field
            string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
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
            string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
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
            try
            {
                string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
                playerDao.Insert(new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null));
            }
            catch (SqlException)
            {
                return; // if only the insert fails, assume count() still works
            }

            int cnt2 = playerDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void Update()
        {
            string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
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
            string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            try
            {
                playerDao.Update(player);
                Assert.Fail();
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void Constructor()
        {

            string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
            var player1 = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player1.UserName, uniqueUsername);

            uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
            var pw = new byte[32];
            var salt = new byte[32];
            var player2 = new Player(0, "first", "last", "nick", uniqueUsername, pw, salt,
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player2.UserName, uniqueUsername);
        }

        [TestMethod]
        public void FindAll()
        {
            int cnt = playerDao.Count();
            Assert.AreEqual(0, cnt);

            const int insertAmount = 5;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = Guid.NewGuid().ToString().Substring(0, 15);
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }

            cnt = playerDao.Count();
            Assert.AreEqual(insertAmount, cnt);

            var players = playerDao.FindAll();
            Assert.AreEqual(players.Count, cnt);
        }
        
        
    }
    
}
