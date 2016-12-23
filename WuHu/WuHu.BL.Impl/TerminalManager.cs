using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class TerminalManager : ITerminalManager
    {
        protected readonly Authenticator Authentication;
        protected readonly IPlayerDao PlayerDao;

        public TerminalManager()
        {
            Authentication = Authenticator.GetInstance();
            PlayerDao = DalFactory.CreatePlayerDao(DalFactory.CreateDatabase());
        }

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
