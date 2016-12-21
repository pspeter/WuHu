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
            var salt1 = CryptoService.GenerateSalt();
            var salt2 = CryptoService.GenerateSalt();
            Assert.IsNotNull(salt1);
            Assert.AreNotEqual(salt1, salt2);
        }

        [TestMethod]
        public void Password()
        {
            const string password = "testpw";
            var salt = CryptoService.GenerateSalt();
            Assert.IsNotNull(salt);
            var hash1 = CryptoService.HashPassword(password, salt);
            var hash2 = CryptoService.HashPassword(password, salt);
            Assert.IsNotNull(hash1);

            Assert.IsTrue(hash1.SequenceEqual(hash2));

            var success = CryptoService.CheckPassword(password, hash1, salt);
            Assert.IsTrue(success);

            success = CryptoService.CheckPassword("wrongPw", hash1, salt);
            Assert.IsFalse(success);

            var wrongSalt = CryptoService.GenerateSalt();
            success = CryptoService.CheckPassword(password, hash1, wrongSalt);
            Assert.IsFalse(success);
        }
    }
}
