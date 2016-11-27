using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Test
{
    [TestClass]
    public class MatchTests
    {
        private static string connectionString;
        private static IDatabase database;
        private static IPlayerDao playerDao;
        private static IMatchDao matchDao;
        private static ITournamentDao tournamentDao;
        private static Player testPlayer1;
        private static Player testPlayer2;
        private static Player testPlayer3;
        private static Player testPlayer4;
        private static Tournament testTournament;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            CommonData.BackupDb();

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            database = DalFactory.CreateDatabase();
            playerDao = DalFactory.CreatePlayerDao(database);
            tournamentDao = DalFactory.CreateTournamentDao(database);
            matchDao = DalFactory.CreateMatchDao(database);

            testPlayer1 = playerDao.FindById(0);
            if (testPlayer1 == null)
            {
                testPlayer1 = new Player("first", "last", "nick", "user1", "pass",
                    false, false, false, false, false, true, true, true, null);
                int playerId = playerDao.Insert(testPlayer1);
            }

            testPlayer2 = playerDao.FindById(1);
            if (testPlayer2 == null)
            {
                testPlayer2 = new Player("first", "last", "nick", "user2", "pass",
                    false, false, false, false, false, true, true, true, null);
                int playerId = playerDao.Insert(testPlayer2);
            }

            testPlayer3 = playerDao.FindById(2);
            if (testPlayer3 == null)
            {
                testPlayer3 = new Player("first", "last", "nick", "user3", "pass",
                    false, false, false, false, false, true, true, true, null);
                int playerId = playerDao.Insert(testPlayer3);
            }
            testPlayer4 = playerDao.FindById(3);
            if (testPlayer4 == null)
            {
                testPlayer4 = new Player("first", "last", "nick", "user4", "pass",
                    false, false, false, false, false, true, true, true, null);
                int playerId = playerDao.Insert(testPlayer4);
            }
            testTournament = tournamentDao.FindById(0);
            if (testTournament == null)
            {
                testTournament = new Tournament("Test", testPlayer1);
                tournamentDao.Insert(testTournament);
            }
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(matchDao);

            Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
            Assert.IsNotNull(match);

            match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
            Assert.IsNotNull(match);

            try
            {
                new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer1, testPlayer3, testPlayer4);
                Assert.Fail("Player can play with or against himseslf");
            }
            catch (ArgumentException) { }

            try
            {
                new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 1.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
                Assert.Fail("Win chance out of range");
            }
            catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void Count()
        {
            int cnt1 = matchDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            matchDao.Insert(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4));

            int cnt2 = matchDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void FindById()
        {
            Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
            int matchId = matchDao.Insert(match);
            Match foundMatch = matchDao.FindById(matchId);

            Assert.AreEqual(match.MatchId, foundMatch.MatchId);
        }

        [TestMethod]
        public void FindAll()
        {
            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
                matchDao.Insert(match);
            }

            int foundAfterInsert = matchDao.FindAll().Count;
            Assert.IsTrue(foundAfterInsert >= insertAmount);
        }

        [TestMethod]
        public void FindAllByPlayer()
        {
            const int insertAmount = 10;
            for (var i = 0; i < insertAmount; ++i)
            {
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
                matchDao.Insert(match);
            }

            int foundAfterInsert = matchDao.FindAllByPlayer(testPlayer1).Count;
            Assert.IsTrue(foundAfterInsert >= insertAmount);
        }

        [TestMethod]
        public void Insert()
        {
            int cnt = matchDao.Count();
            var match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
            var matchId = matchDao.Insert(match);
            int newCnt = matchDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsInstanceOfType(matchId, typeof(int));
            Assert.IsTrue(matchId >= 0);
            Assert.AreEqual(matchId, match.MatchId);
        }

        [TestMethod]
        public void InsertWithoutPlayerIdFails()
        {
            var player = new Player("", "", "", "", "", false, false,
                                false, false, false, false, false, false, null);

            try
            {
                matchDao.Insert(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, player, testPlayer2, testPlayer3, testPlayer4));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            { }
            
            try
            {
                matchDao.Insert(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer1, testPlayer3, testPlayer4));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void PlayerPlayingAgainstHimselfFails()
        {
            try
            {
                matchDao.Insert(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer1, testPlayer3, testPlayer4));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void Update()
        {
            var match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);

            int matchId = matchDao.Insert(match);

            byte newValue = 5;
            match.ScoreTeam1 = newValue;
            matchDao.Update(match);

            match = matchDao.FindById(matchId);
            Assert.AreEqual(newValue, match.ScoreTeam1);
        }

        [TestMethod]
        public void UpdateWithoutPlayerIdFails()
        {
            Player player = playerDao.FindById(0);
            if (player == null)
            {
                player = new Player("first", "last", "nic2k", "us7er", "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(player);
                player.PlayerId = null;
            }
            try
            {
                matchDao.Update(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4));
                    // should throw ArgumentException
                Assert.Fail("ArgumentException not thrown for invalid Player.Update()");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void UpdateWithoutTournamentIdFails()
        {
            Tournament tournament = tournamentDao.FindById(0);
            if (tournament == null)
            {
                tournament = new Tournament("name", testPlayer1);
                tournamentDao.Insert(tournament);
                tournament.TournamentId = null;
            }
            try
            {
                matchDao.Update(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4));
                // should throw ArgumentException
                Assert.Fail("ArgumentException not thrown for invalid Player.Update()");
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
