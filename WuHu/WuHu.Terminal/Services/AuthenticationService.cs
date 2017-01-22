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
        private static readonly IUserManager UserMgr = BLFactory.GetUserManager();


        public static bool Login(string username, string password)
        {
            AuthenticatedUser = UserMgr.FindUser(username, password);
            if (AuthenticatedUser == null || !AuthenticatedUser.IsAdmin)
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
    }
}
