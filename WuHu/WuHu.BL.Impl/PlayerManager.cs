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
        protected readonly Authenticator Authentication;
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
            Authentication = Authenticator.GetInstance();
        }


        // Player
        public bool AddPlayer(Player player, Credentials credentials)
        {
            if (!Authenticate(credentials, true)) // only admins can add players
            {
                return false;
            }
            var inserted = PlayerDao.Insert(player);
            if (inserted)
            {
                ManagerFactory.GetRatingManager().AddCurrentRatingFor(player, credentials);
            }
            return inserted;
        }

        public bool UpdatePlayer(Player player, Credentials credentials)
        {
            if (player.PlayerId == null || credentials == null)
            {
                return false;
            }

            // only admins can update other players and users can't make themselves admin
            if (credentials.Username != player.Username || player.IsAdmin)
            {
                return Authenticate(credentials, true) && PlayerDao.Update(player);
            }
            // normal users can edit themselves
            return Authenticate(credentials, false) && PlayerDao.Update(player);
        }

        public bool ChangePassword(string username, string newPassword, Credentials credentials)
        {
            var player = PlayerDao.FindByUsername(username);

            if ((credentials.Username != player.Username && Authenticate(credentials, true)) ||
                (credentials.Username == player.Username && Authenticate(credentials, false)))
            {
                player.Salt = CryptoService.GenerateSalt();
                player.Password = CryptoService.HashPassword(newPassword, player.Salt);
                return true;
            }
            return false;
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

        public IList<Player> GetRanklist()
        {
            return new List<Player>(
                PlayerDao.FindAll()
                .OrderByDescending(p => RatingDao.FindCurrentRating(p)));
        }


        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return Authentication?.Authenticate(credentials, adminRequired) ?? false;
        }
    }
}
