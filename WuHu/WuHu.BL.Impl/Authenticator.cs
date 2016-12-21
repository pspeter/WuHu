using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class Authenticator
    {
        private readonly IPlayerDao _playerDao;
        private static Authenticator _instance;

        protected Authenticator()
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
        }

        public static Authenticator GetInstance()
        {
            return _instance ?? (_instance = new Authenticator());
        }

        public bool Authenticate(Credentials credentials, bool adminRequired = true)
        {
            var user = _playerDao.FindByUsername(credentials.Username);
            if (user == null)
            {
                return false;
            }
            var correctPw = CryptoService.CheckPassword(credentials.Password, user.Password, user.Salt);

            return correctPw && (!adminRequired || user.IsAdmin);
        }
    }
}
