using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WuHu.Common
{
    public static class PasswordManager
    {
        private static int SALT_SIZE = 32;
        private static int PW_SIZE = 32;
        private static int NR_OF_HASH_ITERATIONS = 10000;
        private static RNGCryptoServiceProvider rngSalt = new RNGCryptoServiceProvider();

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SALT_SIZE];
            rngSalt.GetNonZeroBytes(salt);
            return salt;
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            var hasher = new Rfc2898DeriveBytes(password, salt, NR_OF_HASH_ITERATIONS);
            var hashedPw = hasher.GetBytes(PW_SIZE);
            return hashedPw;
        }

        public static bool CheckPassword(string password, byte[] storedHash, byte[] salt)
        {
            byte[] hashedPw = new byte[PW_SIZE];
            var hasher = new Rfc2898DeriveBytes(password, salt, NR_OF_HASH_ITERATIONS);
            hashedPw = hasher.GetBytes(PW_SIZE);
            return hashedPw.SequenceEqual(storedHash);
        }
    }
}
