using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    [TestClass]
    public class MatchManagerTests
    {
        private static IMatchManager _mgr;
        private static IMatchDao _matchDao;
        private static ITournamentDao _tournamentDao;
        private static IPlayerDao _playerDao;
        private static IRatingDao _ratingDao;
        private static IList<Player> _testPlayers;
        private static Tournament _testTournament;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _ratingDao = DalFactory.CreateRatingDao(database);
            _tournamentDao = DalFactory.CreateTournamentDao(database);
            _playerDao = DalFactory.CreatePlayerDao(database);
            _mgr = ManagerFactory.GetMatchManager();
            var rand = new Random(42);
            _testTournament = new Tournament("", DateTime.Now);
            _testPlayers = new List<Player>();
            for (var i = 0; i < 10; ++i)
            {
                var user = TestHelper.GenerateName();
                var player = new Player(i.ToString(), "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
                _ratingDao.Insert(new Rating(player, DateTime.Now, rand.Next(4000)));
                _testPlayers.Add(player);
            }
        }

        private Match CreateTestMatch()
        {
            return new Match(_testTournament, DateTime.Now, null, null, 0.5, false,
                _testPlayers[0], _testPlayers[1], _testPlayers[2], _testPlayers[3]);
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(_mgr);
        }
        
        [TestMethod]
        public void SetScore()
        {
            var match = CreateTestMatch();

            Assert.IsFalse(match.IsDone);
            Assert.IsNull(match.ScoreTeam1);

            match.ScoreTeam1 = 0;
            match.ScoreTeam2 = 1;
            _mgr.SetScore(match, )
        }
    }
}
