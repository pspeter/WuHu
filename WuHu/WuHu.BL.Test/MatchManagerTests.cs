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
        private static IMatchManager _matchMgr;
        private static IMatchDao _matchDao;
        private static ITournamentDao _tournamentDao;
        private static IPlayerDao _playerDao;
        private static IRatingDao _ratingDao;
        private static IList<Player> _testPlayers;

        internal static string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _ratingDao = DalFactory.CreateRatingDao(database);
            _tournamentDao = DalFactory.CreateTournamentDao(database);
            _playerDao = DalFactory.CreatePlayerDao(database);
            _matchMgr = MatchManager.GetInstance();
            var rand = new Random(42);
            _testPlayers = new List<Player>();
            for (int i = 0; i < 100; ++i)
            {
                var user = GenerateName();
                var player = new Player(i.ToString(), "last", "nick", user, "pass",
                    true, false, false, false, false, true, true, true, null);
                _playerDao.Insert(player);
                _ratingDao.Insert(new Rating(player, DateTime.Now, rand.Next(4000)));
                _testPlayers.Add(player);
            }
        }

        [TestMethod]
        public void CreateMatches()
        {
            var amountMatches = 100;
            Player admin = _testPlayers.First();
            Tournament tournament = new Tournament("", admin);
            _tournamentDao.Insert(tournament);
            var matches = _matchDao.FindAllByTournament(tournament);
            Assert.AreEqual(0, matches.Count);
            var success = _matchMgr.CreateMatches(tournament, _testPlayers, amountMatches, new Credentials(admin.Username, "pass"));
            Assert.IsTrue(success);
            matches = _matchDao.FindAllByTournament(tournament);
            Assert.AreEqual(amountMatches, matches.Count);
        }
    }
}
