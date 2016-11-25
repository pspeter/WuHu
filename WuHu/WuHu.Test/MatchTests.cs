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

            Match match = new Match(testTournament, new DateTime(2000, 1,1), 0, 0, 0.5f, false);
            Assert.IsNotNull(match);

            match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false);
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void Count()
        {
            int cnt1 = matchDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            matchDao.Insert(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false));

            int cnt2 = matchDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
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
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false);
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
            int foundInitial = matchDao.FindAllByPlayer(testPlayer1).Count;
            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                Match match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false);
                matchDao.Insert(match);
            }

            int foundAfterInsert = matchDao.FindAllByPlayer(testPlayer1).Count;
            Assert.AreEqual(foundInitial + insertAmount, foundAfterInsert);
        }

        [TestMethod]
        public void Insert()
        {
            int cnt = matchDao.Count();
            var match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false));
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
                matchDao.Insert(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false));
                Assert.Fail("No ArgumentException thrown.");
            }
            catch (ArgumentException)
            { }
        }

        [TestMethod]
        public void Update()
        {
            var match = new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false);

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
                matchDao.Update(new Match(testTournament, new DateTime(2000, 1, 1), 0, 0, 0.5f, false));
                    // should throw ArgumentException
                Assert.Fail("ArgumentException not thrown for invalid Player.Update()");
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
