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
        private static IRatingManager _ratingMgr;
        private static IMatchDao _matchDao;
        private static ITournamentDao _tournamentDao;
        private static IPlayerDao _playerDao;
        private static IRatingDao _ratingDao;
        private static IScoreParameterDao _paramDao;
        private static IList<Player> _testPlayers;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _ratingDao = DalFactory.CreateRatingDao(database);
            _tournamentDao = DalFactory.CreateTournamentDao(database);
            _playerDao = DalFactory.CreatePlayerDao(database);
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _ratingMgr = RatingManager.GetInstance();
            var rand = new Random(42);

            _testPlayers = new List<Player>();
            for (int i = 0; i < 10; ++i)
            {
                var user = TestHelper.GenerateName();
                var player = new Player(i.ToString(), "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
                _ratingDao.Insert(new Rating(player, DateTime.Now, rand.Next(4000)));
                _testPlayers.Add(player);
            }
        }

        [TestMethod]
        public void Constructor()
        {
            var mgr = RatingManager.GetInstance();
            Assert.IsNotNull(mgr);
            Assert.AreEqual(mgr, _ratingMgr);
        }
        
        [TestMethod]
        public void AddRating()
        {
            var user = TestHelper.GenerateName();
            Player player = new Player("first", "last", "nick", user, "pass",
                true, false, false, false, false, true, true, true, null);
            var creds = new Credentials(user, "pass");
            _playerDao.Insert(player);
            _ratingMgr.AddCurrentRatingFor(player, creds);

            var rating = _ratingDao.FindCurrentRating(player);
            Assert.AreEqual(rating.Value, int.Parse(_paramDao.FindById("initialScore")
                ?.Value ?? DefaultParameter.InitialScore));

            var tournament = new Tournament("", player);
            Assert.IsTrue(_tournamentDao.Insert(tournament));
            var wonMatch = new Match(tournament, DateTime.Now, 10, 0, 0.5, true, 
                player, _testPlayers[0], _testPlayers[1], _testPlayers[2]);
            Assert.IsTrue(_matchDao.Insert(wonMatch));
            _ratingMgr.AddCurrentRatingFor(player, creds);

            var higherRating = _ratingDao.FindCurrentRating(player);
            Assert.IsTrue(higherRating.Value > rating.Value);

            var lostMatch = new Match(tournament, DateTime.Now, 0, 10, 0.5, true,
                player, _testPlayers[0], _testPlayers[1], _testPlayers[2]);
            Assert.IsTrue(_matchDao.Insert(lostMatch));
            _ratingMgr.AddCurrentRatingFor(player, creds);

            var lowerRating = _ratingDao.FindCurrentRating(player);
            Assert.IsTrue(higherRating.Value > lowerRating.Value);
        }
    }
}
