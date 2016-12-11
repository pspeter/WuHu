using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.Test
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

        [AssemblyInitialize] // only one per assembly!
        public static void AssemblyInitialize(TestContext testContext)
        {
            TestHelper.BackupDb();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
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
                playerDao.Insert(testPlayer1);
            }

            testPlayer2 = playerDao.FindById(1);
            if (testPlayer2 == null)
            {
                testPlayer2 = new Player("first", "last", "nick", "user2", "pass",
                    false, false, false, false, false, true, true, true, null);
                 playerDao.Insert(testPlayer2);
            }

            testPlayer3 = playerDao.FindById(2);
            if (testPlayer3 == null)
            {
                testPlayer3 = new Player("first", "last", "nick", "user3", "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(testPlayer3);
            }
            testPlayer4 = playerDao.FindById(3);
            if (testPlayer4 == null)
            {
                testPlayer4 = new Player("first", "last", "nick", "user4", "pass",
                    false, false, false, false, false, true, true, true, null);
                playerDao.Insert(testPlayer4);
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

            Match match = new Match(null, testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
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

            try
            {
                new Match(null, testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer1, testPlayer3, testPlayer4);
                Assert.Fail("Player can play with or against himseslf");
            }
            catch (ArgumentException) { }

            try
            {
                new Match(null, testTournament, new DateTime(2000, 1, 1), 0, 0, 1.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
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
            matchDao.Insert(match);
            Assert.IsNotNull(match.MatchId);
            Match foundMatch = matchDao.FindById(match.MatchId.Value);

            Assert.AreEqual(match.MatchId, foundMatch.MatchId);
        }

        [TestMethod]
        public void FindAll()
        {
            int foundInitial = matchDao.FindAll().Count;
            int cntInitial = matchDao.Count();
            Assert.AreEqual(foundInitial, cntInitial);

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
                matchDao.Insert(match);
            }
            int cntAfterInsert = matchDao.Count();
            Assert.AreEqual(insertAmount + foundInitial, cntAfterInsert);

            int foundAfterInsert = matchDao.FindAll().Count;
            Assert.AreEqual(cntAfterInsert, foundAfterInsert);
        }

        [TestMethod]
        public void FindAllByPlayer()
        {
            const int insertAmount = 10;
            int foundInitial = matchDao.FindAllByPlayer(testPlayer1).Count;
            for (var i = 0; i < insertAmount; ++i)
            {
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
                matchDao.Insert(match);
            }

            int foundAfterInsert = matchDao.FindAllByPlayer(testPlayer1).Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);

            try
            {
                matchDao.FindAllByPlayer(new Player("", "", "", "", "", false, false,
                    false, false, false, false, false, false, null));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void FindAllByTournament()
        {
            const int insertAmount = 10;
            int foundInitial = matchDao.FindAllByTournament(testTournament).Count;

            for (var i = 0; i < insertAmount; ++i)
            {
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
                matchDao.Insert(match);
            }

            int foundAfterInsert = matchDao.FindAllByTournament(testTournament).Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);

            try
            {
                matchDao.FindAllByTournament(new Tournament("name", testPlayer1));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void Insert()
        {
            int cnt = matchDao.Count();
            var match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
            matchDao.Insert(match);
            Assert.IsNotNull(match.MatchId);
            int newCnt = matchDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsTrue(match.MatchId.Value >= 0);
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
        }

        [TestMethod]
        public void InsertWithoutTournamentIdFails()
        {
            Tournament tournamentWithoutId = tournamentDao.FindById(0);
            if (tournamentWithoutId == null)
            {
                tournamentWithoutId = new Tournament("name", testPlayer1);
                tournamentDao.Insert(tournamentWithoutId);
            }
            tournamentWithoutId.TournamentId = null;
            Match match = new Match(tournamentWithoutId, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);

            try
            {
                matchDao.Insert(match);
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            {
            }
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
            matchDao.Insert(match);
            Assert.IsNotNull(match.MatchId);

            byte newValue = 5;
            match.ScoreTeam1 = newValue;
            matchDao.Update(match);

            match = matchDao.FindById(match.MatchId.Value);
            Assert.AreEqual(newValue, match.ScoreTeam1);

            match = matchDao.FindById(-1);
            Assert.IsNull(match);
        }

        [TestMethod]
        public void UpdateWithoutIdFails()
        {
            Player playerWithoutId = new Player("first", "last", "nic2k", "us7er", "pass",
                    false, false, false, false, false, true, true, true, null);

            try // no playerId
            {
                matchDao.Update(new Match(0, testTournament, new DateTime(2000, 1, 1),
                    0, 0, 0.5, false, playerWithoutId, testPlayer2, testPlayer3, testPlayer4));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            {
            }

            Tournament tournamentWithoutId = new Tournament("name", testPlayer1);
            try // no tournamentId
            {
                matchDao.Update(new Match(0, tournamentWithoutId, new DateTime(2000, 1, 1),
                     0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            {
            }

            try // no matchId
            {
                matchDao.Update(new Match(testTournament, new DateTime(2000, 1, 1),
                     0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Delete()
        {
            var match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5, false, testPlayer1, testPlayer2, testPlayer3, testPlayer4);
            matchDao.Insert(match);
            Assert.IsNotNull(match.MatchId);
            var deleted = matchDao.Delete(match);
            Assert.IsTrue(deleted);
            var foundMatch = matchDao.FindById(match.MatchId.Value);
            Assert.IsNull(foundMatch);
            deleted = matchDao.Delete(match);
            Assert.IsFalse(deleted);
        }
    }
}
