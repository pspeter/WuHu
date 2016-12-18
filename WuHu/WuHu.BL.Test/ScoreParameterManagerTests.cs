using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    [TestClass]
    public class ScoreParameterManagerTests
    {
        private static ICommonManager _mgr;
        private static IScoreParameterDao _paramDao;
        private static Credentials _creds;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _mgr = ManagerFactory.GetTerminalManager();
            var user = TestHelper.GenerateName();
            var admin = new Player("admin", "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
            DalFactory.CreatePlayerDao(database).Insert(admin);
            _creds = new Credentials(user, "pass");
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(_mgr);
        }

        [TestMethod]
        public void GetAll()
        {
            var initCnt = _mgr.GetAllParameters().Count;
            for (var i = 0; i < 5; ++i)
            {
                var key = TestHelper.GenerateName();
                _paramDao.Insert(new ScoreParameter(key, ""));
            }

            var newCnt = _mgr.GetAllParameters().Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }

        [TestMethod]
        public void Add()
        {
            var initCnt = _paramDao.FindAll().Count;
            string key;
            for (var i = 0; i < 5; ++i)
            {
                key = TestHelper.GenerateName();
                Assert.IsTrue(_mgr.AddParameter(new ScoreParameter(key, ""), _creds));
            }

            var newCnt = _paramDao.FindAll().Count;
            Assert.AreEqual(initCnt + 5, newCnt);

            key = TestHelper.GenerateName();
            Assert.IsFalse(_mgr.AddParameter(
                new ScoreParameter(key, ""), new Credentials("", "1234")));
        }

        [TestMethod]
        public void Get()
        {
            var key = TestHelper.GenerateName();
            var param = new ScoreParameter(key, "");
            _paramDao.Insert(param);
            var foundParam = _mgr.GetParameter(key);
            Assert.AreEqual(param.Key, foundParam.Key);
            Assert.AreEqual(param.Value, foundParam.Value);
        }

        [TestMethod]
        public void Update()
        {
            var key = TestHelper.GenerateName();
            var param = new ScoreParameter(key, "");
            _paramDao.Insert(param);
            param.Value = "newValue";
            Assert.IsTrue(_mgr.UpdateParameter(param, _creds));
            var foundParam = _paramDao.FindById(param.Key);
            Assert.AreEqual(param.Key, foundParam.Key);
            Assert.AreEqual(param.Value, foundParam.Value);
            Assert.IsFalse(_mgr.UpdateParameter(param, new Credentials("", "1234")));
        }
    }
}
