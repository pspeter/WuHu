using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.Dal.Common;
using WuHu.Domain;

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
            PlayerTestContext = testContext;
            playerDao = DalFactory.CreatePlayerDao(database);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

        }

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }
        

        [TestMethod]
        public void Insert()
        {
            int cnt = playerDao.Count();
            Assert.AreEqual(0, cnt);
            playerDao.Insert(new Player("first", "last", "nick", "user", "pass", 
                false, false, false, false, false, true, true, true, null));
            cnt = playerDao.Count();
            Assert.AreEqual(1, cnt);
        }

        [TestMethod]
        public void Count()
        {

        }

        [TestMethod]
        public void Update()
        {

        }

        [TestMethod]
        public void Constructor()
        {

        }

       
        


    }
    
}
