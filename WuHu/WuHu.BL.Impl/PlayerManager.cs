using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;
using WuHu.BL;

namespace WuHu.BL.Impl
{
    public class PlayerManager : IPlayerManager
    {
        private static PlayerManager _instance;
        private Authenticator _authenticator;
        private readonly IPlayerDao _playerDao;

        protected PlayerManager()
        {
            var database = DalFactory.CreateDatabase();
            _playerDao = DalFactory.CreatePlayerDao(database);
            _authenticator = Authenticator.GetInstance();
        }

        public static PlayerManager GetInstance()
        {
            return _instance ?? (_instance = new PlayerManager());
        }

        public bool AddPlayer(Player player, Credentials credentials)
        {
            if (!Authenticate(credentials, true)) // only admins can add players
            {
                return false;
            }
            return _playerDao.Insert(player);
        }

        public bool UpdatePlayer(Player player, Credentials credentials)
        {
            if (player.PlayerId == null)
            {
                return false;
            }

            if (credentials.Username != player.Username || player.IsAdmin) //only admins can update other players and users can't make themselves admin
            {
                if (!Authenticate(credentials, true))
                {
                    return false;
                }
                return _playerDao.Update(player);
            }

            if (!Authenticate(credentials, false))
            {
                return false;
            }
            return _playerDao.Update(player);
        }

        public IList<Player> GetAllPlayers()
        {
            throw new NotImplementedException();
        }

        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return _authenticator.Authenticate(credentials, adminRequired);
        }
    }
}
