using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;

namespace WuHu.BL.Test
{
    [TestClass]
    public class CommonTests
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            TestHelper.DropTables(database);
            TestHelper.CreateTables(database);
        }
    }
}
