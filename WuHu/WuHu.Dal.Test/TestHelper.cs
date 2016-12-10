using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;

namespace WuHu.Dal.Test
{
    public static class TestHelper
    {
        private static readonly string DbPath = ConfigurationManager.AppSettings["DbPath"];
        private static readonly string DbName = ConfigurationManager.AppSettings["DbName"];
        private static readonly string SqlPath = ConfigurationManager.AppSettings["SqlPath"];

        public static void CreateTables(IDatabase database)
        {
            string script = File.ReadAllText(SqlPath + "dbo.createAll.sql");

            DbCommand cmd = database.CreateCommand(script);
            database.ExecuteNonQuery(cmd);
        }

        internal static string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }

        public static void DropTables(IDatabase database)
        {
            string script = File.ReadAllText(SqlPath + "dbo.dropAll.sql");

            DbCommand cmd = database.CreateCommand(script);
            try
            {
                database.ExecuteNonQuery(cmd);
            }
            catch
            {
                // ignored
            }
        }

        public static void InsertTestData(IDatabase database)
        {
            DropTables(database);
            CreateTables(database);
            IEnumerable<string> script = File.ReadLines(SqlPath + "dbo.Testdata.sql");

            foreach (var line in script)
            {
                Console.WriteLine(line);
                DbCommand cmd = database.CreateCommand(line);
                database.ExecuteNonQuery(cmd);
            }
        }

        internal static void DeleteAllFromTable(IDatabase database, string table)
        {
            DbCommand cmd = database.CreateCommand("DELETE FROM dbo." + table);
            database.ExecuteNonQuery(cmd);
        }

        internal static void BackupDb()
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
