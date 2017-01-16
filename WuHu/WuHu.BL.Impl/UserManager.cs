using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    class UserManager : IUserManager
    {
        protected readonly IPlayerDao PlayerDao;

        public UserManager()
        {
            PlayerDao = DalFactory.CreatePlayerDao(DalFactory.CreateDatabase());
        }

        public Player FindUser(string username, string password)
        {
            var user = PlayerDao.FindByUsername(username);
            if (user == null)
            {
                return null;
            }
            var correctPw = CryptoService.CheckPassword(password, user.Password, user.Salt);

            return correctPw ? user : null;
        }
    }
}
