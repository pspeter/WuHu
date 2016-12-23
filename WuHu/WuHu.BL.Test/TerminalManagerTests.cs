using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    [TestClass]
    public class TerminalManagerTests
    {
        private static IPlayerDao _playerDao;
        private static Credentials _creds;
        private static ITerminalManager _mgr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
            _mgr = ManagerFactory.GetTerminalManager();

            var user = TestHelper.GenerateName();
            _creds = new Credentials(user, "pass");
            _playerDao.Insert(new Player("", "", "", user, "pass", true,
                false, false, false, false, false, false, false, null));
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(_mgr);
            Assert.IsNotNull(new TerminalManager());
        }

        [TestMethod]
        public void Authentication()
        {
            Assert.IsFalse(_mgr.IsUserAuthenticated());
            Assert.IsTrue(_mgr.Login(_creds.Username, _creds.Password));
            Assert.IsTrue(_mgr.IsUserAuthenticated());
            Assert.IsTrue(_mgr.AuthenticatedCredentials.Username.Equals(_creds.Username));
        }
    }
}
