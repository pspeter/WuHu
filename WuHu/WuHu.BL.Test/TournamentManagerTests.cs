using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;
using WuHu.BL;

namespace WuHu.BL.Test
{
    [TestClass]
    public class TournamentManagerTests
    {
        private static ITournamentManager _mgr;
        private static IMatchDao _matchDao;
        private static ITournamentDao _tournamentDao;
        private static IPlayerDao _playerDao;
        private static IRatingDao _ratingDao;
        private static IScoreParameterDao _paramDao;
        private static IList<Player> _testPlayers;
        private static Credentials _creds;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _ratingDao = DalFactory.CreateRatingDao(database);
            _tournamentDao = DalFactory.CreateTournamentDao(database);
            _playerDao = DalFactory.CreatePlayerDao(database);
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _mgr = BLFactory.GetTournamentManager();
            var rand = new Random(42);

            _testPlayers = new List<Player>();
            for (var i = 0; i < 4; ++i)
            {
                var user = TestHelper.GenerateName();
                var player = new Player(i.ToString(), "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
                _ratingDao.Insert(new Rating(player, DateTime.Now, rand.Next(4000)));
                _testPlayers.Add(player);
            }
            _creds = new Credentials(_testPlayers[0].Username, "pass");
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(_mgr);
        }

        [TestMethod]
        public void CreateMatches()
        {
            const int amountMatches = 3;
            var admin = _testPlayers.First();
            var tournament = new Tournament("", DateTime.Now);
            _tournamentDao.Insert(tournament);
            var matches = _matchDao.FindAllByTournament(tournament);
            Assert.AreEqual(0, matches.Count);
            _mgr.LockTournament(_creds);
            var success = _mgr.CreateTournament(tournament, new List<Player>(_testPlayers), amountMatches, new Credentials(admin.Username, "pass"));
            _mgr.UnlockTournament(_creds);
            Assert.IsTrue(success);
            matches = _matchDao.FindAllByTournament(tournament);
            Assert.AreEqual(amountMatches, matches.Count);
        }

        [TestMethod]
        public void GetAllTournaments()
        {
            var foundInitial = _mgr.GetAllTournaments().Count;
            var cntInitial = _tournamentDao.Count();
            Assert.AreEqual(foundInitial, cntInitial);

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                var tournament = new Tournament("name", DateTime.Now);
                _tournamentDao.Insert(tournament);
            }
            var cntAfterInsert = _tournamentDao.Count();
            Assert.AreEqual(insertAmount + foundInitial, cntAfterInsert);

            var foundAfterInsert = _mgr.GetAllTournaments().Count;
            Assert.AreEqual(cntAfterInsert, foundAfterInsert);
        }

        [TestMethod]
        public void GetMostRecent()
        {
            var t = new Tournament("", DateTime.Now);
            _tournamentDao.Insert(t);

            var mostRecent = _mgr.GetMostRecentTournament();

            Assert.IsTrue(t.TournamentId == mostRecent.TournamentId);
        }

        [TestMethod]
        public void UpdateTournament()
        {
            var t = new Tournament("", DateTime.Now);
            _mgr.LockTournament(_creds);
            _mgr.CreateTournament(t, new List<Player>(_testPlayers), 1, _creds);
            Assert.IsNotNull(t.TournamentId);

            _mgr.LockTournament(_creds);
            Assert.IsTrue(_mgr.UpdateTournament(t, new List<Player>(_testPlayers), 2, _creds));
            var matchesAmount = _matchDao.FindAllByTournament(t).Count;

            Assert.AreEqual(2, matchesAmount);
        }
    }
}
