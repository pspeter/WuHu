﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using WuHu.Dal.Common;

namespace WuHu.Test
{
    public static class CommonData
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

        public static void InsertTestData(IDatabase database)
        {
            DropTables(database);
            CreateTables(database);
            IEnumerable<string> script = File.ReadLines(@"C:\Users\Peter\Documents\Sourcetree\WuHu\SQL_scripts\dbo.Testdata.sql");

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
    }
}
