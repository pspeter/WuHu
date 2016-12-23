using System;
using System.Configuration;
using System.IO;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Test
{
    internal static class TestHelper
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
            catch (Exception)
            {
                // ignored
            }
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
