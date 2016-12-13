using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class MatchManager : IMatchManager
    {
        private static MatchManager _instance;
        private readonly Authenticator _authenticator;
        private readonly IMatchDao _matchDao;

        protected MatchManager()
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _authenticator = Authenticator.GetInstance();
        }

        public static MatchManager GetInstance()
        {
            return _instance ?? (_instance = new MatchManager());
        }

        public bool SetScore(Match match, Credentials credentials)
        {
            if (!Authenticate(credentials, true) || match.MatchId == null)
            {
                return false;
            }

            var oldMatch = _matchDao.FindById(match.MatchId.Value);

            if (oldMatch.IsDone)
            {
                return false;
            }

            var ratingMgr = RatingManager.GetInstance();
            oldMatch.ScoreTeam1 = match.ScoreTeam1;
            oldMatch.ScoreTeam2 = match.ScoreTeam2;
            oldMatch.IsDone = true;
            var updated = _matchDao.Update(oldMatch);

            if (!updated)
            {
                return false;
            }

            ratingMgr.AddCurrentRatingFor(match.Player1, credentials);
            ratingMgr.AddCurrentRatingFor(match.Player2, credentials);
            ratingMgr.AddCurrentRatingFor(match.Player3, credentials);
            ratingMgr.AddCurrentRatingFor(match.Player4, credentials);
            return true;
        }

        public IList<Match> GetAllMatches()
        {
            return _matchDao.FindAll();
        }

        public IList<Match> GetAllMatchesFor(Player player)
        {
            return _matchDao.FindAllByPlayer(player);
        }

        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return _authenticator.Authenticate(credentials, adminRequired);
        }
    }
}
