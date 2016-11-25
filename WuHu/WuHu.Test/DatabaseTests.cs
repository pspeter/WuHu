using System;
using System.Configuration;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;

namespace WuHu.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private static IDatabase database;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            CommonData.BackupDb();

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            database = DalFactory.CreateDatabase();
        }


        [TestMethod]
        public void DeclareParameterTwice()
        {
            var cmd = database.CreateCommand("SELECT * FROM Player WHERE playerId = @playerId;");
            database.DeclareParameter(cmd, "playerId", DbType.Int32);

            try
            {
                database.DeclareParameter(cmd, "playerId", DbType.Int32);
                Assert.Fail("Declaring parameter twice");
            }
            catch(ArgumentException) { }
        }

        [TestMethod]
        public void SetParameter()
        {
            var cmd = database.CreateCommand("SELECT * FROM Player WHERE playerId = @playerId;");
            database.DeclareParameter(cmd, "playerId", DbType.Int32);
            database.SetParameter(cmd, "playerId", 0);
            try
            {
                database.SetParameter(cmd, "invalid", "");
                Assert.Fail("Set invalid parameter");
            }
            catch (ArgumentException) { }
        }
    }
}
