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
    public class RatingManagerTests
    {
        private static IRatingManager _mgr;
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
            _mgr = ManagerFactory.GetRatingManager();
            var rand = new Random(42);

            _testPlayers = new List<Player>();
            for (var i = 0; i < 3; ++i)
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
        public void AddCurrentRating()
        {
            var user = TestHelper.GenerateName();
            var player = new Player("first", "last", "nick", user, "pass",
                true, false, false, false, false, true, true, true, null);
            _playerDao.Insert(player);
            _mgr.AddCurrentRatingFor(player, _creds);

            var rating = _ratingDao.FindCurrentRating(player);
            Assert.AreEqual(rating.Value, int.Parse(_paramDao.FindById("initialScore")
                ?.Value ?? DefaultParameter.InitialScore));

            var tournament = new Tournament("", DateTime.Now);
            Assert.IsTrue(_tournamentDao.Insert(tournament));
            var wonMatch = new Match(tournament, DateTime.Now, 0, 10, 0.5, true, 
                _testPlayers[0], _testPlayers[1], player, _testPlayers[2]);
            Assert.IsTrue(_matchDao.Insert(wonMatch));
            _mgr.AddCurrentRatingFor(player, _creds);

            var higherRating = _ratingDao.FindCurrentRating(player);
            Assert.IsTrue(higherRating.Value > rating.Value);

            var lostMatch = new Match(tournament, DateTime.Now, 0, 10, 0.5, true,
                player, _testPlayers[0], _testPlayers[1], _testPlayers[2]);
            Assert.IsTrue(_matchDao.Insert(lostMatch));
            _mgr.AddCurrentRatingFor(player, _creds);

            var lowerRating = _ratingDao.FindCurrentRating(player);
            Assert.IsTrue(higherRating.Value > lowerRating.Value);

            var matchWithoutScores = new Match(tournament, DateTime.Now, null, null, 0.5, true,
                player, _testPlayers[0], _testPlayers[1], _testPlayers[2]);
            _matchDao.Insert(matchWithoutScores);
            Assert.IsTrue(_matchDao.Insert(lostMatch));
            try
            {
                _mgr.AddCurrentRatingFor(player, _creds);
                Assert.Fail("No Exception thrown");
            }
            catch (ArgumentException) { }
            matchWithoutScores.ScoreTeam1 = 0;
            matchWithoutScores.ScoreTeam2 = 10;
            _matchDao.Update(matchWithoutScores);

            Assert.IsFalse(_mgr.AddCurrentRatingFor(player, new Credentials("", "1234")));
        }

        [TestMethod]
        public void GetCurrent()
        {
            var rating = 623;
            var datetime = DateTime.Now;
            _ratingDao.Insert(new Rating(_testPlayers[0], datetime, rating));
            var foundRating = _mgr.GetCurrentRatingFor(_testPlayers[0]);
            Assert.AreEqual(rating, foundRating.Value);
        }

        [TestMethod]
        public void GetAll()
        {
            var initCnt = _mgr.GetAllRatings().Count;
            for (var i = 0; i < 5; ++i)
            {
                _ratingDao.Insert(new Rating(_testPlayers[0], DateTime.Now, 0));
            }

            var newCnt = _mgr.GetAllRatings().Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }

        [TestMethod]
        public void GetAllFor()
        {
            var initCnt = _mgr.GetAllRatingsFor(_testPlayers[0]).Count;
            for (var i = 0; i < 5; ++i)
            {
                _ratingDao.Insert(new Rating(_testPlayers[0], DateTime.Now, 0));
            }

            var newCnt = _mgr.GetAllRatingsFor(_testPlayers[0]).Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }

        [TestMethod]
        public void Add()
        {
            var initCnt = _ratingDao.FindAll().Count;
            for (var i = 0; i < 5; ++i)
            {
                _mgr.AddCurrentRatingFor(_testPlayers[0], _creds);
            }

            var newCnt = _ratingDao.FindAll().Count;
            Assert.AreEqual(initCnt + 5, newCnt);
        }
    }
}
