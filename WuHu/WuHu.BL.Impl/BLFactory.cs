using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.BL.Impl
{
    public static class BLFactory
    {
        private static IMatchManager _matchManager;
        private static IRatingManager _ratingManager;
        private static IScoreParameterManager _scoreParameterManager;
        private static IPlayerManager _playerManager;
        private static ITournamentManager _tournamentManager;
        private static IUserManager _userManager;
        

        public static IMatchManager GetMatchManager()
        {
            return _matchManager ?? (_matchManager = new MatchManager());
        }
        public static IRatingManager GetRatingManager()
        {
            return _ratingManager ?? (_ratingManager = new RatingManager());
        }

        public static IScoreParameterManager GetScoreParameterManager()
        {
            return _scoreParameterManager ?? (_scoreParameterManager = new ScoreParameterManager());
        }

        public static IPlayerManager GetPlayerManager()
        {
            return _playerManager ?? (_playerManager = new PlayerManager());
        }

        public static ITournamentManager GetTournamentManager()
        {
            return _tournamentManager ?? (_tournamentManager = new TournamentManager());
        }
        public static IUserManager GetUserManager()
        {
            return _userManager ?? (_userManager = new UserManager());
        }
    }
}
