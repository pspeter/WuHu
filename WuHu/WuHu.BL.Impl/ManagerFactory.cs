﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.BL.Impl
{
    public static class ManagerFactory
    {
        private static ITerminalManager _terminalManager;
        private static IMatchManager _matchManager;
        private static IRatingManager _ratingManager;
        private static IScoreParameterManager _scoreParameterManager;
        private static IPlayerManager _playerManager;
        private static ITournamentManager _tournamentManager;

        public static ITerminalManager GetTerminalManager()
        {
            return _terminalManager ?? (_terminalManager = new TerminalManager());
        }

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
    }
}
