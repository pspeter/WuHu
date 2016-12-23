using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.SqlServer
{
    public class ScoreParameterDao : IScoreParameterDao
    {
        private const string SqlFindById =
          @"SELECT *
            FROM ScoreParameter
            WHERE [key] = @key;";

        private const string SqlFindAll =
            @"SELECT *
            FROM ScoreParameter;";

        private const string SqlInsert =
            @"INSERT INTO ScoreParameter
                VALUES (@key, @value);";

        private const string SqlUpdate =
            @"UPDATE ScoreParameter
                SET value = @value
                WHERE [key] = @key";

        private readonly IDatabase database;

        public ScoreParameterDao(IDatabase database)
        {
            this.database = database;
        }


        protected DbCommand CreateFindAllCmd()
        {
            return database.CreateCommand(SqlFindAll);
        }

        public IList<ScoreParameter> FindAll()
        {
            using (var command = CreateFindAllCmd())
            using (var reader = database.ExecuteReader(command))
            {
                var result = new List<ScoreParameter>();
                while (reader.Read())
                {
                    result.Add(new ScoreParameter((string) reader["key"], 
                                                  (string) reader["value"]));
                }
                return result;
            }
        }
        protected DbCommand CreateFindByIdCmd(string key)
        {
            var findByIdCmd = database.CreateCommand(SqlFindById);
            database.DefineParameter(findByIdCmd, "key", DbType.String, key);
            return findByIdCmd;
        }

        public ScoreParameter FindById(string key)
        {
            using (var command = CreateFindByIdCmd(key))
            using (var reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new ScoreParameter((string)reader["key"],
                                              (string)reader["value"]);
                }
                else
                {
                    return null;
                }
            }
        }

        private DbCommand CreateInsertCmd(string key, string value)
        {
            var cmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(cmd, "key", DbType.String, key);
            database.DefineParameter(cmd, "value", DbType.String, value);
            return cmd;
        }

        public bool Insert(ScoreParameter param)
        {
            using (var command = CreateInsertCmd(param.Key, param.Value))
            {
                return (database.ExecuteNonQuery(command) == 1);
            }
        }

        private DbCommand CreateUpdateCmd(string key, string value)
        {
            var cmd = database.CreateCommand(SqlUpdate);
            database.DefineParameter(cmd, "key", DbType.String, key);
            database.DefineParameter(cmd, "value", DbType.String, value);
            return cmd;
        }

        public bool Update(ScoreParameter param)
        {
            using (var command = CreateUpdateCmd(param.Key, param.Value))
            {
                return (database.ExecuteNonQuery(command) == 1);
            }
        }
    }
}
