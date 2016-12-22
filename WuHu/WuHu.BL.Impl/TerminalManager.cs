using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class TerminalManager : CommonManager, ITerminalManager
    {

        public bool Login(string username, string password)
        {
            AuthenticatedCredentials = new Credentials(username, password);

            if (!Authentication.Authenticate(AuthenticatedCredentials, true))
            {
                Logout();
                return false;
            }

            AuthenticatedUser = PlayerDao.FindByUsername(username);
            return true;
        }
        
        public bool IsUserAuthenticated()
        {
            return AuthenticatedUser != null;
        }

        public void Logout()
        {
            AuthenticatedUser = null;
            AuthenticatedCredentials = null;
        }

        public Credentials AuthenticatedCredentials { get; private set; }

        public Player AuthenticatedUser { get; private set; }
    }
}
