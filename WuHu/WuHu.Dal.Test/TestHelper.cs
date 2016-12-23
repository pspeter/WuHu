using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.Test
{
    public static class TestHelper
    {
        private static readonly string DbPath = ConfigurationManager.AppSettings["DbPath"];
        private static readonly string DbName = ConfigurationManager.AppSettings["DbName"];
        private static readonly string SqlPath = ConfigurationManager.AppSettings["SqlPath"];

        public static void CreateTables(IDatabase database)
        {
            var script = File.ReadAllText(SqlPath + "dbo.createAll.sql");

            var cmd = database.CreateCommand(script);
            database.ExecuteNonQuery(cmd);
        }

        internal static string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }

        public static void DropTables(IDatabase database)
        {
            var script = File.ReadAllText(SqlPath + "dbo.dropAll.sql");

            var cmd = database.CreateCommand(script);
            try
            {
                database.ExecuteNonQuery(cmd);
            }
            catch(Exception)
            {
                // ignored
            }
        }

        public static void InsertTestData(IDatabase database)
        {
            DropTables(database);
            CreateTables(database);
            var playerDao = DalFactory.CreatePlayerDao(database);

            playerDao.Insert(new Player("Herwald", "Müller", "Lego-las", "legologo", "legoistcool", true, 
                true, true, false, false, true, true, true, null));

            playerDao.Insert(new Player("Heinz", "Janda", "Javanda", "JavaJanda", "javaistcool", true,
                false, false, false, false, true, true, true, null));

            playerDao.Insert(new Player("Dobrila", "Reiter", "Pascal", "compiling...", "cistcool", true,
                false, false, false, false, true, true, true, null));

            playerDao.Insert(new Player("Trinke", "Fanta", "Sei Kambucha", "secretelypepsi", "droptableplayer", true,
                false, false, false, false, true, true, true, null));

            playerDao.Insert(new Player("Mark", "Spinner", "Deitscher", "oleeee", "bayernistcool", true,
                false, false, false, false, true, true, true, null));

            var rand = new Random();
            
            /*
            for (var i = 0; i < 25; ++i)
            {
                var uniqueUsername = GenerateName();

                playerDao.Insert(new Player("Vorname", "Nachname", "Spitzname", uniqueUsername, "passwort", false, 
                    rand.NextDouble() > 0.4, rand.NextDouble() > 0.4, rand.NextDouble() > 0.4, rand.NextDouble() > 0.4,
                    rand.NextDouble() > 0.4, rand.NextDouble() > 0.4, rand.NextDouble() > 0.4, null));
            }*/
            
            var script = File.ReadLines(SqlPath + "dbo.Testdata.sql");
            foreach (var line in script)
            {
                Console.WriteLine(line);
                var cmd = database.CreateCommand(line);
                try
                {
                    database.ExecuteNonQuery(cmd);
                }
                catch
                {
                    Console.WriteLine("error");
                }
            }
        }

        internal static void DeleteAllFromTable(IDatabase database, string table)
        {
            var cmd = database.CreateCommand("DELETE FROM dbo." + table);
            database.ExecuteNonQuery(cmd);
        }

        internal static void BackupDb(IDatabase database)
        {
            try
            {
                if (!File.Exists(DbPath + DbName + ".mdf.bak"))
                {
                    File.Copy(DbPath + DbName + ".mdf", DbPath + DbName + ".mdf.bak", true);
                    File.Copy(DbPath + DbName + "_log.ldf", DbPath + DbName + "_log.ldf.bak", true);
                }
                else
                {
                    File.Copy(DbPath + DbName + ".mdf.bak", DbPath + DbName + ".mdf", true);
                    File.Copy(DbPath + DbName + "_log.ldf.bak", DbPath + DbName + "_log.ldf", true);
                }
            }
            catch (IOException)
            {
                Console.Write("Warning: Database backup not possible. Already in use.");
            }
        }
    }
}
