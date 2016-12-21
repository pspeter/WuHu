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
        private Player _authenticatedUser;
        private Credentials _credentials;

        public bool Login(string username, string password)
        {
            _credentials = new Credentials(username, password);

            if (!Authentication.Authenticate(_credentials, true))
            {
                Logout();
                return false;
            }

            _authenticatedUser = PlayerDao.FindByUsername(username);
            return true;
        }
        
        public bool IsUserAuthenticated()
        {
            return true;// _authenticatedUser != null;
        }

        public void Logout()
        {
            _authenticatedUser = null;
            _credentials = null;
        }

        public Credentials GetUserCredentials()
        {
            return _credentials;
        }
    }
}
