using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.SqlServer
{
    public class TournamentDao : ITournamentDao
    {

        private const string SqlFindById =
          @"SELECT * 
            FROM Tournament
            WHERE tournamentId = @tournamentId;";

        private const string SqlFindAll =
            @"SELECT * 
                FROM Tournament;";

        private const string SqlFindMostRecent =
            @"SELECT TOP(1) *
                FROM Tournament
                ORDER BY Tournament.datetime DESC;";

        private const string SqlInsert =
            @"INSERT INTO Tournament (name, datetime)
                OUTPUT Inserted.tournamentId
                VALUES (@name, @datetime);";

        private const string SqlCount =
            @"SELECT Count(*)
                FROM Tournament;";

        private const string SqlUpdate =
            @"UPDATE Tournament
                SET datetime = @datetime,
                    name = @name
                WHERE tournamentId = @tournamentId";

        private readonly IDatabase database;

        public TournamentDao(IDatabase database)
        {
            this.database = database;
        }

        protected DbCommand CreateFindAllCmd()
        {
            return database.CreateCommand(SqlFindAll);
        }

        public IList<Tournament> FindAll()
        {
            using (var command = CreateFindAllCmd())
            using (var reader = database.ExecuteReader(command))
            {
                var result = new List<Tournament>();
                while (reader.Read())
                    result.Add(new Tournament((int)reader["tournamentId"],
                                              (string)reader["name"],
                                              (DateTime)reader["datetime"]));
                return result;
            }
        }

        protected DbCommand CreateFindMostRecent()
        {
            var findByIdCmd = database.CreateCommand(SqlFindMostRecent);
            return findByIdCmd;
        }

        protected DbCommand CreateFindByIdCmd(int tournamentId)
        {
            var findByIdCmd = database.CreateCommand(SqlFindById);
            database.DefineParameter(findByIdCmd, "tournamentId", DbType.Int32, tournamentId);
            return findByIdCmd;
        }

        public Tournament FindMostRecentTournament()
        {
            using (var command = CreateFindMostRecent())
            using (var reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Tournament((int)reader["tournamentId"],
                                          (string)reader["name"],
                                          (DateTime)reader["datetime"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public Tournament FindById(int tournamentId)
        {
            using (var command = CreateFindByIdCmd(tournamentId))
            using (var reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Tournament((int)reader["tournamentId"],
                                          (string)reader["name"],
                                          (DateTime)reader["datetime"]);
                }
                else
                {
                    return null;
                }
            }
        }

        private DbCommand CreateInsertCmd(string name, DateTime datetime)
        {
            var cmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(cmd, "name", DbType.String, name);
            database.DefineParameter(cmd, "datetime", DbType.DateTime2, datetime);
            return cmd;
        }

        public bool Insert(Tournament tournament)
        {

            using (var command = CreateInsertCmd(tournament.Name, tournament.Datetime))
            {
                try
                {
                    var id = database.ExecuteScalar(command); // set the objects id right away
                    tournament.TournamentId = id;
                }
                catch (SqlException)
                {
                    return false;
                }
                return true;
            }
        }
        
        protected DbCommand CreateUpdateCmd(int tournamentId, string name, DateTime datetime)
        {
            var updateByIdCmd = database.CreateCommand(SqlUpdate);
            database.DefineParameter(updateByIdCmd, "tournamentId", DbType.Int32, tournamentId);
            database.DefineParameter(updateByIdCmd, "name", DbType.String, name);
            database.DefineParameter(updateByIdCmd, "datetime", DbType.DateTime2, datetime);

            return updateByIdCmd;
        }

        public bool Update(Tournament tournament)
        {
            if (tournament.TournamentId == null)
            {
                throw new ArgumentException("TournamentId null on update for Tournament");
            }
            using (var command = CreateUpdateCmd(tournament.TournamentId.Value, 
                tournament.Name, tournament.Datetime))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        private DbCommand CreateCountCmd()
        {
            return database.CreateCommand(SqlCount);
        }
        public int Count()
        {
            using (var command = CreateCountCmd())
            {
                return database.ExecuteScalar(command);
            }
        }
    }
}
