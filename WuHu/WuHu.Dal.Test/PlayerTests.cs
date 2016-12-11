using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.Dal.Common;
using WuHu.Domain;
using System.Data.SqlClient;
using System.IO;
using WuHu.Common;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class PlayerTests
    {
        private static IPlayerDao _playerDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
        }
        
        [TestMethod]
        public void FindById()
        {
            string uniqueUsername = TestHelper.GenerateName();
            Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            _playerDao.Insert(player);
            Assert.IsNotNull(player.PlayerId);
            Player foundPlayer = _playerDao.FindById(player.PlayerId.Value);

            Assert.AreEqual(player.PlayerId, foundPlayer.PlayerId);

            Player nullPlayer = _playerDao.FindById(-1);
            Assert.IsNull(nullPlayer);
        }

        [TestMethod]
        public void Insert()
        {
            //generates a random string for our user field
            string uniqueUsername = TestHelper.GenerateName();
            int cnt = _playerDao.Count();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            var inserted = _playerDao.Insert(player);
            Assert.IsTrue(inserted);

            inserted = _playerDao.Insert(player);
            Assert.IsFalse(inserted);

            int newCnt = _playerDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsNotNull(player.PlayerId);
            Assert.IsTrue(player.PlayerId.Value >= 0);
        }

        [TestMethod]
        public void InsertUserUniqueConstraint()
        {
            string uniqueUsername = TestHelper.GenerateName();
            var player = new Player("", "", "", uniqueUsername, "", false, false, 
                                false, false, false, false, false, false, null);
            _playerDao.Insert(player);
            var inserted = _playerDao.Insert(player);
            Assert.IsFalse(inserted);
        }

        [TestMethod]
        public void Count()
        {
            int cnt1 = _playerDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            string uniqueUsername = TestHelper.GenerateName();
            _playerDao.Insert(new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null));

            int cnt2 = _playerDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void Update()
        {
            string uniqueUsername = TestHelper.GenerateName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);

            _playerDao.Insert(player);
            Assert.IsNotNull(player.PlayerId);

            string newFirst = "newFirst";
            player.PlayerId = player.PlayerId;
            player.Firstname = newFirst;
            _playerDao.Update(player);

            player = _playerDao.FindById(player.PlayerId.Value);
            Assert.AreEqual(newFirst, player.Firstname);
        }

        [TestMethod]
        public void UpdateWithoutPlayerIdFails()
        {
            string uniqueUsername = TestHelper.GenerateName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            try
            {
                _playerDao.Update(player); // should throw ArgumentException
                Assert.Fail("ArgumentException not thrown for invalid Player.Update()");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void Constructor()
        {

            string uniqueUsername = TestHelper.GenerateName();
            var player1 = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player1.Username, uniqueUsername);
            Assert.IsNull(player1.PlayerId);
            Assert.IsNotNull(player1.Password);
            Assert.IsNotNull(player1.Salt);

            string builtPlayerString = player1.Firstname + " '" + player1.Nickname + "' " + player1.Lastname;
            Assert.AreEqual(builtPlayerString, player1.ToString());

            uniqueUsername = TestHelper.GenerateName();
            var pw = new byte[32];
            var salt = new byte[32];
            var player2 = new Player(0, "first", "last", "nick", uniqueUsername, pw, salt,
                false, false, false, false, false, true, true, true, null);
            Assert.AreEqual(player2.Username, uniqueUsername);

            var hashCode = player2.GetHashCode();
            Assert.IsNotNull(hashCode);

            Assert.IsFalse(player1.Equals(player2));
            Assert.IsTrue(player1.Equals(player1));
        }

        [TestMethod]
        public void FindAll()
        {
            int cntInital = _playerDao.Count();

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = TestHelper.GenerateName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
            }

            int cntAfterInsert = _playerDao.Count();
            Assert.AreEqual(insertAmount + cntInital, cntAfterInsert);

            var players = _playerDao.FindAll();
            Assert.AreEqual(players.Count, insertAmount + cntInital);
        }

        [TestMethod]
        public void FindAllByString()
        {
            string uniqueFirstName = TestHelper.GenerateName();

            int totalInital = _playerDao.Count();
            int foundInitial = _playerDao.FindAllByString(uniqueFirstName).Count;

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = TestHelper.GenerateName();
                Player player = new Player(uniqueFirstName, "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
            }


            int totalAfterFirstInsert = _playerDao.Count();
            Assert.AreEqual(insertAmount + totalInital, totalAfterFirstInsert);
            int foundAfterFirstInsert = _playerDao.FindAllByString(uniqueFirstName).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterFirstInsert);

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = TestHelper.GenerateName();
                Player player = new Player("other", "other", "other", uniqueUsername, "pass",
                    false, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
            }

            int totalAfterSecondInsert = _playerDao.Count();
            Assert.AreEqual(insertAmount * 2 + totalInital, totalAfterSecondInsert);

            var foundAfterSecondInsert = _playerDao.FindAllByString(uniqueFirstName).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterSecondInsert);
        }

        [TestMethod]
        public void FindByUsername()
        {
            string uniqueUsername = TestHelper.GenerateName();
            Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                false, false, false, false, false, true, true, true, null);
            _playerDao.Insert(player);
            Player foundPlayer = _playerDao.FindByUsername(uniqueUsername);

            Assert.AreEqual(player.PlayerId, foundPlayer.PlayerId);

            uniqueUsername = TestHelper.GenerateName();
            Player nullPlayer = _playerDao.FindByUsername(uniqueUsername);
            Assert.IsNull(nullPlayer);
        }

        [TestMethod]
        public void FindAllByDay()
        {
            int cntInital = _playerDao.FindAllOnDays(monday: true).Count;

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = TestHelper.GenerateName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, false, false, false, false, false, false, false, null);
                _playerDao.Insert(player);
            }


            int cntAfterFirstInsert = _playerDao.FindAllOnDays(monday: true).Count;
            Assert.AreEqual(cntInital, cntAfterFirstInsert);

            for (var i = 0; i < insertAmount; ++i)
            {
                string uniqueUsername = TestHelper.GenerateName();
                Player player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, true, true, true, true, true, true, true, null);
                _playerDao.Insert(player);
            }

            int cntAfterSecondInsert = _playerDao.FindAllOnDays(monday: true).Count;
            Assert.AreEqual(insertAmount + cntInital, cntAfterSecondInsert);
        }

        [TestMethod]
        public void ChangePassword()
        {
            string newPw = "newPw";

            string uniqueUsername = TestHelper.GenerateName();
            var player = new Player("first", "last", "nick", uniqueUsername, "pass",
                    false, true, true, true, true, true, true, true, null);
            player.ChangePassword(newPw);

            bool success = PasswordManager.CheckPassword(newPw, player.Password, player.Salt);

            Assert.IsTrue(success);
        }
    }
}
