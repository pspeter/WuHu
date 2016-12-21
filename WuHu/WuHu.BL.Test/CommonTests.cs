using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WuHu.BL.Test
{
    [TestClass]
    public class CommonTests
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            TestHelper.BackupDb();
        }
    }
}
