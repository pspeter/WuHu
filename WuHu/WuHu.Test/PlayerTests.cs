using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.Dal.Common;
using WuHu.Domain;
using System.Data.SqlClient;
using System.IO;
using WuHu.Common;

namespace WuHu.Test
{
    [TestClass]
    public class PlayerTests
    {
        private static IDatabase database;
        private static IPlayerDao playerDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            CommonData.BackupDb();
            
            database = DalFactory.CreateDatabase();
            playerDao = DalFactory.CreatePlayerDao(database);
        }

        private string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }
        
        [TestMethod]
        public void FindById()
        {
            string uniqueUsername = GenerateName();
            Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            int playerId = playerDao.Insert(player);
            Player foundPlayer = playerDao.FindById(playerId);

            Assert.AreEqual(player.PlayerId, foundPlayer.PlayerId);
        }

        [TestMethod]
        public void Insert()
        {
            //generates a random string for our user field
            string uniqueUsername = GenerateName();
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
            string uniqueUsername = GenerateName();
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
            string uniqueUsername = GenerateName();
            playerDao.Insert(new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null));

            int cnt2 = playerDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void Update()
        {
            string uniqueUsername = GenerateName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);

            int playerId = playerDao.Insert(player);

            string newFirst = "newFirst";
            player.PlayerId = playerId;
            player.Firstname = newFirst;
            playerDao.Update(player);

            player = playerDao.FindById(playerId);
            Assert.AreEqual(newFirst, player.Firstname);
        }

        [TestMethod]
        public void UpdateWithoutPlayerIdFails()
        {
            string uniqueUsername = GenerateName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            try
            {
                playerDao.Update(player); // should throw ArgumentException
                Assert.Fail("ArgumentException not thrown for invalid Player.Update()");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void Constructor()
        {

            string uniqueUsername = GenerateName();
            var player1 = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player1.Username, uniqueUsername);
            Assert.IsNull(player1.PlayerId);
            
            string builtPlayerString = player1.Firstname + " '" + player1.Nickname + "' " + player1.Lastname;
            Assert.AreEqual(builtPlayerString, player1.ToString());

            uniqueUsername = GenerateName();
            var pw = new byte[32];
            var salt = new byte[32];
            var player2 = new Player(0, "first", "last", "nick", uniqueUsername, pw, salt,
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player2.Username, uniqueUsername);
        }

        [TestMethod]
        public void FindAll()
        {
            int cntInital = playerDao.Count();

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }

            int cntAfterInsert = playerDao.Count();
            Assert.AreEqual(insertAmount + cntInital, cntAfterInsert);

            var players = playerDao.FindAll();
            Assert.AreEqual(players.Count, insertAmount + cntInital);
        }

        [TestMethod]
        public void FindAllByString()
        {
            string uniqueFirstName = GenerateName();

            int totalInital = playerDao.Count();
            int foundInitial = playerDao.FindAllByString(uniqueFirstName).Count;

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateName();
                Player player = new Player(uniqueFirstName, "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }


            int totalAfterFirstInsert = playerDao.Count();
            Assert.AreEqual(insertAmount + totalInital, totalAfterFirstInsert);
            int foundAfterFirstInsert = playerDao.FindAllByString(uniqueFirstName).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterFirstInsert);

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateName();
                Player player = new Player("other", "other", "other", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
            }

            int totalAfterSecondInsert = playerDao.Count();
            Assert.AreEqual(insertAmount * 2 + totalInital, totalAfterSecondInsert);

            var foundAfterSecondInsert = playerDao.FindAllByString(uniqueFirstName).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterSecondInsert);
        }

        [TestMethod]
        public void FindAllByDay()
        {
            int cntInital = playerDao.FindAllOnDays(monday: true).Count;

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, false, false, false, null);
                playerDao.Insert(player);
            }


            int cntAfterFirstInsert = playerDao.FindAllOnDays(monday: true).Count;
            Assert.AreEqual(cntInital, cntAfterFirstInsert);

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = GenerateName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, true, true, true, true, true, true, true, null);
                playerDao.Insert(player);
            }

            int cntAfterSecondInsert = playerDao.FindAllOnDays(monday: true).Count;
            Assert.AreEqual(insertAmount + cntInital, cntAfterSecondInsert);
        }

        [TestMethod]
        public void ChangePassword()
        {
            string newPw = "newPw";

            string uniqueUsername = GenerateName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, true, true, true, true, true, true, true, null);
            player.ChangePassword(newPw);

            bool success = PasswordManager.CheckPassword(newPw, player.Password, player.Salt);

            Assert.IsTrue(success);
        }
    }
}
