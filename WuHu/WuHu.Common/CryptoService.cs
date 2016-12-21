using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WuHu.Common
{
    public static class CryptoService
    {
        private const int SaltSize = 32;
        private const int PwSize = 32;
        private const int NrOfHashIterations = 10000;
        private static readonly RNGCryptoServiceProvider SaltGenerator = new RNGCryptoServiceProvider();

        public static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            SaltGenerator.GetNonZeroBytes(salt);
            return salt;
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            var hasher = new Rfc2898DeriveBytes(password, salt, NrOfHashIterations);
            var hashedPw = hasher.GetBytes(PwSize);
            return hashedPw;
        }

        public static bool CheckPassword(string password, byte[] storedHash, byte[] salt)
        {
            var hasher = new Rfc2898DeriveBytes(password, salt, NrOfHashIterations);
            var hashedPw = hasher.GetBytes(PwSize);
            return hashedPw.SequenceEqual(storedHash);
        }
    }
}
