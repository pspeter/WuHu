using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;

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
        public void Create()
        { 
        }
    }
}
