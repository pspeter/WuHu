using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Common;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class PasswordManagerTests
    {
        

        [TestMethod]
        public void Salt()
        {
            byte[] salt1 = PasswordManager.GenerateSalt();
            byte[] salt2 = PasswordManager.GenerateSalt();
            Assert.IsNotNull(salt1);
            Assert.AreNotEqual(salt1, salt2);
        }

        [TestMethod]
        public void Password()
        {
            string password = "testpw";
            byte[] salt = PasswordManager.GenerateSalt();
            Assert.IsNotNull(salt);
            byte[] hash1 = PasswordManager.HashPassword(password, salt);
            byte[] hash2 = PasswordManager.HashPassword(password, salt);
            Assert.IsNotNull(hash1);

            Assert.IsTrue(hash1.SequenceEqual(hash2));

            bool success = PasswordManager.CheckPassword(password, hash1, salt);
            Assert.IsTrue(success);

            success = PasswordManager.CheckPassword("wrongPw", hash1, salt);
            Assert.IsFalse(success);

            byte[] wrongSalt = PasswordManager.GenerateSalt();
            success = PasswordManager.CheckPassword(password, hash1, wrongSalt);
            Assert.IsFalse(success);
        }
    }
}
