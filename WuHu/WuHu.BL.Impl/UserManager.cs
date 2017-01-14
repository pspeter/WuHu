using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    class UserManager : IUserManager
    {
        protected readonly Authenticator Authentication;
        protected readonly IPlayerDao PlayerDao;

        public UserManager()
        {
            Authentication = Authenticator.GetInstance();
            PlayerDao = DalFactory.CreatePlayerDao(DalFactory.CreateDatabase());
        }

        public Player FindUser(string username, string password)
        {
            var creds = new Credentials(username, password);
            if (!Authentication.Authenticate(creds, false))
            {
                return null;
            }
            return PlayerDao.FindByUsername(username);
        }
    }
}
