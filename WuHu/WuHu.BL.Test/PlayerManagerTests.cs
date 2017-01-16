using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    [TestClass]
    public class PlayerManagerTests
    {
        private static IPlayerManager _mgr;
        private static IPlayerDao _playerDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
            _mgr = BLFactory.GetPlayerManager();
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(_mgr);
    
            var player = new Player("", "", "", "", "", false,
                false, false, false, false, false, false, false, null);

            Assert.IsNotNull(player);
            Assert.IsNotNull(player.Username);

            player = new Player(0, "", "", "", "", new byte[32], new byte[32], false,
                false, false, false, false, false, false, false, null);

            Assert.IsNotNull(player);
            Assert.IsNotNull(player.Username);
        }

        [TestMethod]
        public void GetAll()
        {
            var initCnt = _mgr.GetAllPlayers().Count;
            var key = TestHelper.GenerateName();
            for (var i = 0; i < 5; ++i)
            {
                var user = TestHelper.GenerateName();
                var player = new Player("admin", "last", "nick", user, "pass",
                        true, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
            }

            var newCnt = _mgr.GetAllPlayers().Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }

        [TestMethod]
        public void GetById()
        {
            var user = TestHelper.GenerateName();
            var player = new Player("admin", "last", "nick", user, "pass",
                        true, false, false, false, false, true, true, true, null);
            _playerDao.Insert(player);
            Assert.IsNotNull(player.PlayerId);
            var foundPlayer = _mgr.GetPlayer(player.PlayerId.Value);
            Assert.AreEqual(player.PlayerId, foundPlayer.PlayerId);
            Assert.AreEqual(player.Username, foundPlayer.Username);
        }

        [TestMethod]
        public void GetByUser()
        {
            var user = TestHelper.GenerateName();
            var player = new Player("admin", "last", "nick", user, "pass",
                        true, false, false, false, false, true, true, true, null);
            _playerDao.Insert(player);
            Assert.IsNotNull(player.PlayerId);
            var foundPlayer = _mgr.GetPlayer(player.Username);
            Assert.AreEqual(player.PlayerId, foundPlayer.PlayerId);
            Assert.AreEqual(player.Username, foundPlayer.Username);
        }

        [TestMethod]
        public void Add()
        {
            var user = TestHelper.GenerateName();
            var admin = new Player("admin", "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);

            var player = new Player("NotAdmin", "last", "nick", user, "pass",
                    false, false, false, false, false, true, true, true, null);
            var success = _mgr.AddPlayer(player);
            Assert.IsFalse(success);

            
        }

        [TestMethod]
        public void Update()
        {
            var user = TestHelper.GenerateName();
            var player = new Player("first", "last", "nick", user, "pass",
                    false, false, false, false, false, true, true, true, null);

            user = TestHelper.GenerateName();
            var otherPlayer = new Player("first", "last", "nick", user, "pass",
                    false, false, false, false, false, true, true, true, null);

            Assert.IsTrue(_playerDao.Insert(player));
            Assert.IsTrue(_playerDao.Insert(otherPlayer));

            // players can change their own values
            player.Nickname = "newNick";
            var success = _mgr.UpdatePlayer(player);
            Assert.IsTrue(success);

            // normal players can't change other players' values
            otherPlayer.Nickname = "newNick";
            success = _mgr.UpdatePlayer(otherPlayer);
            Assert.IsFalse(success);

            // normal players can't make themselves admin
            player.IsAdmin = true;
            success = _mgr.UpdatePlayer(player);
            Assert.IsFalse(success);
            
            // other admins can change other players and even make them admin
            success = _mgr.UpdatePlayer(player);
            Assert.IsTrue(success);


            user = TestHelper.GenerateName();
            var notInsertedPlayer = new Player("first", "last", "nick", user, "pass",
                    false, false, false, false, false, true, true, true, null);
            // can't update players that aren't inserted
            success = _mgr.UpdatePlayer(notInsertedPlayer);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void ChangePassword()
        {
            var user = TestHelper.GenerateName();
            var player = new Player("first", "last", "nick", user, "pass",
                    false, false, false, false, false, true, true, true, null);
            _playerDao.Insert(player);
            Assert.IsTrue(_mgr.ChangePassword(user, "newPass"));

            player = _playerDao.FindByUsername(user);

            var success = CryptoService.CheckPassword("newPass", player.Password, player.Salt);

            Assert.IsTrue(success);

        }
    }
}
