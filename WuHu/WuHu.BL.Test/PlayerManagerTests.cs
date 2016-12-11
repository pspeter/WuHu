using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    [TestClass]
    public class PlayerManagerTests
    {
        private static IPlayerManager _playerMgr;
        private static IPlayerDao _playerDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
            _playerMgr = PlayerManager.GetInstance();
        }

        [TestMethod]
        public void Constructor()
        {
            var mgr = PlayerManager.GetInstance();
            Assert.IsNotNull(mgr);
            Assert.AreEqual(mgr, _playerMgr);
        }

        [TestMethod]
        public void Create()
        {
            var user = TestHelper.GenerateName();
            var admin = new Player("admin", "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
            var success = _playerDao.Insert(admin);
            Assert.IsTrue(success);
            var creds = new Credentials(user, "pass");

            var player = new Player("NotAdmin", "last", "nick", user, "pass",
                    false, false, false, false, false, true, true, true, null);
            success = _playerMgr.AddPlayer(player, creds);
            Assert.IsFalse(success);

        }
    }
}
