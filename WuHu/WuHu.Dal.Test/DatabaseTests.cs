using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private static IDatabase _database;

        [AssemblyInitialize] // only one per assembly!
        public static void AssemblyInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            //TestHelper.InsertTestData(database);
            TestHelper.DropTables(database);
            TestHelper.CreateTables(database);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _database = DalFactory.CreateDatabase();
        }


        [TestMethod]
        public void DeclareParameterTwice()
        {
            var cmd = _database.CreateCommand("SELECT * FROM Player WHERE playerId = @playerId;");
            _database.DeclareParameter(cmd, "playerId", DbType.Int32);

            try
            {
                _database.DeclareParameter(cmd, "playerId", DbType.Int32);
                Assert.Fail("Declaring parameter twice");
            }
            catch(ArgumentException) { }
        }

        [TestMethod]
        public void SetParameter()
        {
            var cmd = _database.CreateCommand("SELECT * FROM Player WHERE playerId = @playerId;");
            _database.DeclareParameter(cmd, "playerId", DbType.Int32);
            _database.SetParameter(cmd, "playerId", 0);
            try
            {
                _database.SetParameter(cmd, "invalid", "");
                Assert.Fail("Set invalid parameter");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void InvalidCommand()
        {
            var cmd = _database.CreateCommand("ABCDEFG");
            try
            {
                _database.ExecuteReader(cmd);
                Assert.Fail("No SqlException thrown");
            } catch (SqlException) { }

            try
            {
                _database.ExecuteNonQuery(cmd);
                Assert.Fail("No SqlException thrown");
            }
            catch (SqlException) { }

            try
            {
                _database.ExecuteScalar(cmd);
                Assert.Fail("No SqlException thrown");
            }
            catch (SqlException) { }
        }
    }
}
