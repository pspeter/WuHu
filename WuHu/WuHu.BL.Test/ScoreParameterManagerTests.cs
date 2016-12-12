using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    [TestClass]
    class ScoreParameterManagerTests
    {
        private static IScoreParameterManager _paramMgr;
        private static IScoreParameterDao _paramDao;
        private static Credentials _creds;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _paramMgr = ScoreParameterManager.GetInstance();
            var user = TestHelper.GenerateName();
            var admin = new Player("admin", "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
            _creds = new Credentials(user, "pass");
        }

        [TestMethod]
        public void Constructor()
        {
            var mgr = ScoreParameterManager.GetInstance();
            Assert.IsNotNull(mgr);
            Assert.AreEqual(mgr, _paramMgr);
        }

        [TestMethod]
        public void GetAll()
        {
            var initCnt = _paramMgr.GetAllParameters().Count;
            var key = TestHelper.GenerateName();
            for (var i = 0; i < 5; ++i)
            {
                _paramDao.Insert(new ScoreParameter(key, ""));
            }

            var newCnt = _paramMgr.GetAllParameters().Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }

        [TestMethod]
        public void Add()
        {
            var initCnt = _paramDao.FindAll().Count;
            var key = TestHelper.GenerateName();
            for (var i = 0; i < 5; ++i)
            {
                _paramMgr.AddParameter(new ScoreParameter(key, ""), _creds);
            }

            var newCnt = _paramDao.FindAll().Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }
    }
}
