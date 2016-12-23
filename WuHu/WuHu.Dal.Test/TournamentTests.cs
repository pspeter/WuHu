using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Domain;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class TournamentTests
    {
        
        private static IDatabase database;
        private static IPlayerDao playerDao;
        private static ITournamentDao tournamentDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            database = DalFactory.CreateDatabase();
            playerDao = DalFactory.CreatePlayerDao(database);
            tournamentDao = DalFactory.CreateTournamentDao(database);
        }

        [TestMethod]
        public void Constructor()
        {
            var name = "name";
            Assert.IsNotNull(tournamentDao);

            var tournament = new Tournament(0, name, DateTime.Now);
            Assert.IsNotNull(tournament);

            tournament = new Tournament(name, DateTime.Now);
            Assert.IsNotNull(tournament);

            Assert.AreEqual(tournament.ToString(), name);
        }

        [TestMethod]
        public void Count()
        {
            var cnt1 = tournamentDao.Count();
            Assert.IsTrue(cnt1 >= 0);
            tournamentDao.Insert(new Tournament("name", DateTime.Now));

            var cnt2 = tournamentDao.Count();
            Assert.AreEqual(cnt1 + 1, cnt2);
        }

        [TestMethod]
        public void FindById()
        {
            var tournament = new Tournament("name", DateTime.Now);
            tournamentDao.Insert(tournament);
            Assert.IsNotNull(tournament.TournamentId);
            var foundTournament = tournamentDao.FindById(tournament.TournamentId.Value);

            Assert.AreEqual(tournament.TournamentId, foundTournament.TournamentId);

            var nullTournament = tournamentDao.FindById(-1);
            Assert.IsNull(nullTournament);
        }

        [TestMethod]
        public void FindAll()
        {
            var foundInitial = tournamentDao.FindAll().Count;
            var cntInitial = tournamentDao.Count();
            Assert.AreEqual(foundInitial, cntInitial);

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                var tournament = new Tournament("name", DateTime.Now);
                tournamentDao.Insert(tournament);
            }
            var cntAfterInsert = tournamentDao.Count();
            Assert.AreEqual(insertAmount + foundInitial, cntAfterInsert);

            var foundAfterInsert = tournamentDao.FindAll().Count;
            Assert.AreEqual(cntAfterInsert, foundAfterInsert);
        }

        [TestMethod]
        public void Insert()
        {
            var cnt = tournamentDao.Count();
            var tournament = new Tournament("name", DateTime.Now);
            tournamentDao.Insert(tournament);
            Assert.IsNotNull(tournament.TournamentId);
            var newCnt = tournamentDao.Count();
            Assert.AreEqual(cnt + 1, newCnt);
            Assert.IsTrue(tournament.TournamentId.Value >= 0);
        }


        [TestMethod]
        public void Update()
        {
            var tournament = new Tournament("name", DateTime.Now);

            tournamentDao.Insert(tournament);
            Assert.IsNotNull(tournament.TournamentId);

            var newName = "newName";
            tournament.Name = newName;
            tournamentDao.Update(tournament);

            tournament = tournamentDao.FindById(tournament.TournamentId.Value);
            Assert.AreEqual(newName, tournament.Name);

            
        }

        [TestMethod]
        public void UpdateWithoutIdFails()
        {
            try // no tournamentId
            {
                tournamentDao.Update(new Tournament("name", DateTime.Now));
                Assert.Fail("No ArgumentException thrown");
            }
            catch (ArgumentException) { }
        }
    }
}
