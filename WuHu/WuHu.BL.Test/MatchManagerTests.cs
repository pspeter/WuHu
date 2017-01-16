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
            _mgr = BLFactory.GetMatchManager();
            var rand = new Random(42);
            _testTournament = new Tournament("", DateTime.Now);
            _tournamentDao.Insert(_testTournament);
            _testPlayers = new List<Player>();

            for (var i = 0; i < 10; ++i)
            {
                var user = TestHelper.GenerateName();
                var player = new Player(i.ToString(), "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
                Assert.IsTrue(_playerDao.Insert(player));
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
            _matchDao.Insert(match);

            Assert.IsNotNull(match.MatchId);
            Assert.IsFalse(match.IsDone);
            Assert.IsNull(match.ScoreTeam1);
            Assert.IsNull(match.ScoreTeam1);

            match.ScoreTeam1 = 0;
            match.ScoreTeam2 = 1;
            _mgr.SetScore(match);
            
            match = _matchDao.FindById(match.MatchId.Value);

            Assert.IsFalse(match.IsDone);
            Assert.IsNotNull(match.ScoreTeam1);
        }

        [TestMethod]
        public void GetAll()
        {
            var foundInitial = _mgr.GetAllMatches().Count;
            var cntInitial = _matchDao.Count();
            Assert.AreEqual(foundInitial, cntInitial);

            const int insertAmount = 5;

            for (var i = 0; i < insertAmount; ++i)
            {
                var match = new Match(_testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5,
                    false, _testPlayers[0], _testPlayers[1], _testPlayers[2], _testPlayers[3]);
                _matchDao.Insert(match);
            }
            var cntAfterInsert = _matchDao.Count();
            Assert.AreEqual(insertAmount + foundInitial, cntAfterInsert);

            var foundAfterInsert = _mgr.GetAllMatches().Count;
            Assert.AreEqual(cntAfterInsert, foundAfterInsert);
        }


        [TestMethod]
        public void GetAllByPlayer()
        {
            const int insertAmount = 10;
            var foundInitial = _mgr.GetAllMatchesFor(_testPlayers[1]).Count;
            for (var i = 0; i < insertAmount; ++i)
            {
                var match = new Match(_testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, 
                    _testPlayers[0], _testPlayers[1], _testPlayers[2], _testPlayers[3]);
                _matchDao.Insert(match);
            }

            var foundAfterInsert = _mgr.GetAllMatchesFor(_testPlayers[1]).Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);

            try
            {
                _mgr.GetAllMatchesFor(new Player("", "", "", "", "", false, false,
                    false, false, false, false, false, false, null));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void GetAllByTournament()
        {
            const int insertAmount = 10;
            var foundInitial = _mgr.GetAllMatchesFor(_testTournament).Count;

            for (var i = 0; i < insertAmount; ++i)
            {
                var match = new Match(_testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false,
                    _testPlayers[0], _testPlayers[1], _testPlayers[2], _testPlayers[3]);
                _matchDao.Insert(match);
            }

            var foundAfterInsert = _mgr.GetAllMatchesFor(_testTournament).Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);

            try
            {
                _mgr.GetAllMatchesFor(new Tournament("name", DateTime.Now));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException) { }
        }


        [TestMethod]
        public void Insert()
        {
            var cnt = _matchDao.Count();
            var match = new Match(_testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false,
                _testPlayers[0], _testPlayers[1], _testPlayers[2], _testPlayers[3]);
            _matchDao.Insert(match);
            Assert.IsNotNull(match.MatchId);
            var newCnt = _matchDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsTrue(match.MatchId.Value >= 0);
        }


        [TestMethod]
        public void GetAllUnfinished()
        {
            const int insertAmount = 5;
            var foundInitial = _mgr.GetAllUnfinishedMatches().Count;

            for (var i = 0; i < insertAmount; ++i)
            {
                var match = new Match(_testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false,
                    _testPlayers[0], _testPlayers[1], _testPlayers[2], _testPlayers[3]);
                _matchDao.Insert(match);
            }

            var foundAfterInsert = _mgr.GetAllUnfinishedMatches().Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);
        }
    }
}
