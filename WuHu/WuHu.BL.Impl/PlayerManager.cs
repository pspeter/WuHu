using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;
using WuHu.BL;

namespace WuHu.BL.Impl
{
    class PlayerManager : IPlayerManager
    {
        private Authenticator _authenticator;
        private readonly IPlayerDao _playerDao;

        public PlayerManager()
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
            _authenticator = Authenticator.GetAuthenticator();
        }

        public bool AddPlayer(Player player, string username, string password)
        {
            if (!Authenticate(username, password, true))
            {
                return false;
            }
            return _playerDao.Insert(player);
        }

        public bool UpdatePlayer(Player player, string username, string password)
        {
            throw new NotImplementedException();
        }

        private bool Authenticate(string username, string password, bool adminRequired)
        {
            return _authenticator.Authenticate(username, password, adminRequired);
        }
    }
}
