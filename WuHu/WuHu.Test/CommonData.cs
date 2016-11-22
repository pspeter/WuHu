using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        internal static void BeginTransaction(IDatabase database)
        {
            DbCommand cmd = database.CreateCommand("BEGIN TRANSACTION;");
            database.ExecuteNonQuery(cmd);
        }

        internal static void Commit(IDatabase database)
        {
            DbCommand cmd = database.CreateCommand("COMMIT;");
            database.ExecuteNonQuery(cmd);
        }
        internal static void Rollback(IDatabase database)
        {
            DbCommand cmd = database.CreateCommand("ROLLBACK;");
            database.ExecuteNonQuery(cmd);
        }
    }
}
