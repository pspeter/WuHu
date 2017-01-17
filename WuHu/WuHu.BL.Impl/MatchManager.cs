using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class MatchManager : IMatchManager
    {
        protected readonly IMatchDao MatchDao;
        protected readonly IPlayerDao PlayerDao;
        protected readonly ITournamentDao TournamentDao;
        protected readonly IScoreParameterDao ParamDao;
        protected readonly IRatingDao RatingDao;
        protected readonly IRatingManager RatingManager;
        protected readonly Random Rand = new Random();
        protected bool TournamentLocked;

        public MatchManager()
        {
            var database = DalFactory.CreateDatabase();
            MatchDao = DalFactory.CreateMatchDao(database);
            PlayerDao = DalFactory.CreatePlayerDao(database);
            RatingDao = DalFactory.CreateRatingDao(database);
            TournamentDao = DalFactory.CreateTournamentDao(database);
            ParamDao = DalFactory.CreateScoreParameterDao(database);
            RatingManager = BLFactory.GetRatingManager();
        }

        public bool SetScore(Match match)
        {
            if (match.MatchId == null || 
                match.Player1.PlayerId == null || match.Player2.PlayerId == null || 
                match.Player3.PlayerId == null || match.Player4.PlayerId == null)
            {
                return false;
            }

            var oldMatch = MatchDao.FindById(match.MatchId.Value);

            if (oldMatch.IsDone)
            {
                return false;
            }

            // copying values over to oldMatch to avoid accidently changing any other values
            oldMatch.ScoreTeam1 = match.ScoreTeam1;
            oldMatch.ScoreTeam2 = match.ScoreTeam2;

            return MatchDao.Update(oldMatch);
        }

        // Match
        public bool SetFinalScore(Match match)
        {
            if (match.MatchId == null || match.ScoreTeam1 == match.ScoreTeam2)
            {
                return false;
            }

            var oldMatch = MatchDao.FindById(match.MatchId.Value);

            if (oldMatch.IsDone)
            {
                return false;
            }

            // copying values over to oldMatch to avoid accidently changing any other values
            oldMatch.ScoreTeam1 = match.ScoreTeam1;
            oldMatch.ScoreTeam2 = match.ScoreTeam2;
            oldMatch.IsDone = true;
            match.IsDone = true;

            var updated = MatchDao.Update(oldMatch);

            if (!updated)
            {
                return false;
            }

            RatingManager.AddCurrentRatingFor(match.Player1);
            RatingManager.AddCurrentRatingFor(match.Player2);
            RatingManager.AddCurrentRatingFor(match.Player3);
            RatingManager.AddCurrentRatingFor(match.Player4);
            return true;
        }

        public Match GetCurrentMatchFor(string username)
        {
            var player = PlayerDao.FindByUsername(username);
            if (player?.PlayerId == null)
            {
                return null;
            }
            var match = MatchDao.FindCurrentByPlayer(player.PlayerId.Value);
            return match != null && !match.IsDone ? match : null;
        }

        public IList<Match> GetAllMatches()
        {
            return MatchDao.FindAll();
        }

        public IList<Match> GetAllMatchesFor(Player player)
        {
            return MatchDao.FindAllByPlayer(player);
        }

        public IList<Match> GetAllMatchesFor(Tournament tournament)
        {
            return MatchDao.FindAllByTournament(tournament);
        }

        public IList<Match> GetAllUnfinishedMatches()
        {
            return new List<Match>(MatchDao.FindAll().Where(m => !m.IsDone));
        }
    }
}
