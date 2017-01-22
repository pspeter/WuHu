using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Terminal.Services
{
    static class AuthenticationService
    {
        private static readonly IPlayerManager PlayerMgr = BLFactory.GetPlayerManager();


        public static bool Login(string username, string password)
        {
            if (!Authenticate(username, password))
            {
                Logout();
                return false;
            }

            AuthenticatedUser = PlayerMgr.GetPlayer(username);
            if (!AuthenticatedUser.IsAdmin)
            {
                Logout();
                return false;
            }
            return true;
        }

        public static bool IsAuthenticated()
        {
            return AuthenticatedUser != null && AuthenticatedUser.IsAdmin;
        }

        public static void Logout()
        {
            AuthenticatedUser = null;
        }

        public static Player AuthenticatedUser { get; private set; }

        private static bool Authenticate(string username, string password)
        {
            if (username == null || password == null) return false;
            var user = PlayerMgr.GetPlayer(username);
            return user != null && 
                CryptoService.CheckPassword(password, user.Password, user.Salt);
        }
    }
}
