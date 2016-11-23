using System.Data.Common;
using System.IO;
using WuHu.Dal.Common;

namespace WuHu.Test
{
    static class CommonData
    {
        internal static void CreateTables(IDatabase database)
        {
            string script = File.ReadAllText(@"C:\Users\Peter\Documents\Sourcetree\WuHu\SQL_scripts\dbo.createAll.sql");

            DbCommand cmd = database.CreateCommand(script);
            database.ExecuteNonQuery(cmd);
        }

        internal static void DropTables(IDatabase database)
        {
            string script = File.ReadAllText(@"C:\Users\Peter\Documents\Sourcetree\WuHu\SQL_scripts\dbo.dropAll.sql");

            DbCommand cmd = database.CreateCommand(script);
            database.ExecuteNonQuery(cmd);
        }

        internal static void InsertTestData(IDatabase database)
        {
            string script = File.ReadAllText(@"C:\Users\Peter\Documents\Sourcetree\WuHu\SQL_scripts\dbo.Testdata.sql");

            DbCommand cmd = database.CreateCommand(script);
            database.ExecuteNonQuery(cmd);
        }

        internal static void DeleteAllFromTable(IDatabase database, string table)
        {
            DbCommand cmd = database.CreateCommand("DELETE FROM dbo." + table);
            database.ExecuteNonQuery(cmd);
        }
    }
}
