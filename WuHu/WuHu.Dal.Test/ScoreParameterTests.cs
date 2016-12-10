using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class ScoreParameterTests
    {
        private static IDatabase database;
        private static IScoreParameterDao paramDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            database = DalFactory.CreateDatabase();
            paramDao = DalFactory.CreateScoreParameterDao(database);
        }
        
        private string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(paramDao);

            string id = GenerateName();
            ScoreParameter param = new ScoreParameter(id, "val");
            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void FindById()
        {
            string id = GenerateName();
            ScoreParameter param = new ScoreParameter(id, "val");
            paramDao.Insert(param);
            ScoreParameter foundParam = paramDao.FindById(id);

            Assert.AreEqual(param.Value, foundParam.Value);
            Assert.AreEqual(param.Key, foundParam.Key);


            ScoreParameter nullParam = paramDao.FindById("");
            Assert.IsNull(nullParam);
        }

        [TestMethod]
        public void FindAll()
        {
            int foundInitial = paramDao.FindAll().Count;

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                string id = GenerateName();
                ScoreParameter param = new ScoreParameter(id, "val");
                paramDao.Insert(param);
            }

            int foundAfterInsert = paramDao.FindAll().Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);
        }
        
        [TestMethod]
        public void Insert()
        {
            string id = GenerateName();
            ScoreParameter param = new ScoreParameter(id, "val");
            paramDao.Insert(param);

            ScoreParameter foundParam = paramDao.FindById(id);

            Assert.AreEqual(param.Value, foundParam.Value);
            Assert.AreEqual(param.Key, foundParam.Key);
        }

        [TestMethod]
        public void Update()
        {
            string id = GenerateName();
            ScoreParameter param = new ScoreParameter(id, "val");
            paramDao.Insert(param);

            var newValue = "newVal";
            param.Value = newValue;
            paramDao.Update(param);

            param = paramDao.FindById(id);
            Assert.AreEqual(newValue, param.Value);
        }
    }
}
