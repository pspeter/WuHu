using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class PlayerManager : IPlayerManager
    {
        protected readonly IMatchDao MatchDao;
        protected readonly IPlayerDao PlayerDao;
        protected readonly ITournamentDao TournamentDao;
        protected readonly IScoreParameterDao ParamDao;
        protected readonly IRatingDao RatingDao;
        protected readonly Random Rand = new Random();
        protected bool TournamentLocked;

        public PlayerManager()
        {
            var database = DalFactory.CreateDatabase();
            MatchDao = DalFactory.CreateMatchDao(database);
            PlayerDao = DalFactory.CreatePlayerDao(database);
            RatingDao = DalFactory.CreateRatingDao(database);
            TournamentDao = DalFactory.CreateTournamentDao(database);
            ParamDao = DalFactory.CreateScoreParameterDao(database);
        }


        // Player
        public bool AddPlayer(Player player)
        {
            var inserted = PlayerDao.Insert(player);
            if (inserted)
            {
                BLFactory.GetRatingManager().AddCurrentRatingFor(player);
            }
            return inserted;
        }

        public bool UpdatePlayer(Player player)
        {
            if (player.PlayerId == null)
            {
                return false;
            }

            return PlayerDao.Update(player);
        }

        public bool ChangePassword(string username, string newPassword)
        {
            var player = PlayerDao.FindByUsername(username);
            
            player.Salt = CryptoService.GenerateSalt();
            player.Password = CryptoService.HashPassword(newPassword, player.Salt);
            return PlayerDao.Update(player);
        }

        public Player GetPlayer(int playerId)
        {
            return PlayerDao.FindById(playerId);
        }

        public Player GetPlayer(string username)
        {
            return PlayerDao.FindByUsername(username);
        }

        public IList<Player> GetAllPlayers()
        {
            return PlayerDao.FindAll();
        }
    }
}
