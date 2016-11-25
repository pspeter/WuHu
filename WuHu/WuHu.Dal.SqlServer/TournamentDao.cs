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
    class TournamentDao : ITournamentDao
    {

        private const string SqlFindById =
          @"SELECT * 
            FROM Tournament JOIN Player on (Tournament.creator = Player.playerId)
            WHERE tournamentId = @tournamentId;";

        private const string SqlFindAll =
            @"SELECT * 
                FROM Tournament JOIN Player on (Tournament.creator = Player.playerId);";

        private const string SqlInsert =
            @"INSERT INTO Tournament (name, creator)
                OUTPUT Inserted.tournamentId
                VALUES (@name, @creator);";

        private const string SqlCount =
            @"SELECT Count(*)
                FROM Tournament JOIN Player on (Tournament.creator = Player.playerId);";

        private const string SqlUpdate =
            @"UPDATE Tournament
                SET creator = @creator,
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
            using (DbCommand command = CreateFindAllCmd())
            using (IDataReader reader = database.ExecuteReader(command))
            {
                var result = new List<Tournament>();
                while (reader.Read())
                    result.Add(new Tournament((int)reader["tournamentId"],
                                              (string)reader["name"],
                                                new Player((int)reader["playerId"],
                                                    (string)reader["firstName"],
                                                    (string)reader["lastName"],
                                                    (string)reader["nickName"],
                                                    (string)reader["userName"],
                                                    (byte[])reader["password"],
                                                    (byte[])reader["salt"],
                                                    (bool)reader["isAdmin"],
                                                    (bool)reader["playsMondays"],
                                                    (bool)reader["playsTuesdays"],
                                                    (bool)reader["playsWednesdays"],
                                                    (bool)reader["playsThursdays"],
                                                    (bool)reader["playsFridays"],
                                                    (bool)reader["playsSaturdays"],
                                                    (bool)reader["playsSundays"],
                                                    reader.IsDBNull(reader.GetOrdinal("picture")) ?
                                                        null : (byte[])reader["picture"])));
                return result;
            }
        }

        protected DbCommand CreateFindByIdCmd(int tournamentId)
        {
            DbCommand findByIdCmd = database.CreateCommand(SqlFindById);
            database.DefineParameter(findByIdCmd, "tournamentId", DbType.Int32, tournamentId);
            return findByIdCmd;
        }

        public Tournament FindById(int tournamentId)
        {
            using (DbCommand command = CreateFindByIdCmd(tournamentId))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Tournament((int)reader["tournamentId"],
                                          (string)reader["name"],
                                          new Player((int)reader["playerId"],
                                              (string)reader["firstName"],
                                              (string)reader["lastName"],
                                              (string)reader["nickName"],
                                              (string)reader["userName"],
                                              (byte[])reader["password"],
                                              (byte[])reader["salt"],
                                              (bool)reader["isAdmin"],
                                              (bool)reader["playsMondays"],
                                              (bool)reader["playsTuesdays"],
                                              (bool)reader["playsWednesdays"],
                                              (bool)reader["playsThursdays"],
                                              (bool)reader["playsFridays"],
                                              (bool)reader["playsSaturdays"],
                                              (bool)reader["playsSundays"],
                                              reader.IsDBNull(reader.GetOrdinal("picture")) ?
                                                  null : (byte[])reader["picture"]));
                }
                else
                {
                    return null;
                }
            }
        }

        private DbCommand CreateInsertCmd(string name, int playerId)
        {
            DbCommand cmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(cmd, "name", DbType.String, name);
            database.DefineParameter(cmd, "creator", DbType.Int32, playerId);
            return cmd;
        }

        public int Insert(Tournament tournament)
        {
            if (tournament.Creator?.PlayerId == null)
            {
                throw new ArgumentException("No playerId for Insert into Tournament provided.");
            }

            using (DbCommand command = CreateInsertCmd(tournament.Name, tournament.Creator.PlayerId.Value))
            {
                var id = database.ExecuteScalar(command); // set the objects id right away
                tournament.TournamentId = id;
                return id;
            }
        }

        // Update
        protected DbCommand CreateUpdateCmd(int tournamentId, string name, int playerId)
        {
            DbCommand updateByIdCmd = database.CreateCommand(SqlUpdate);
            database.DefineParameter(updateByIdCmd, "tournamentId", DbType.Int32, tournamentId);
            database.DefineParameter(updateByIdCmd, "name", DbType.String, name);
            database.DefineParameter(updateByIdCmd, "creator", DbType.Int32, playerId);

            return updateByIdCmd;
        }

        public bool Update(Tournament tournament)
        {
            if (tournament.TournamentId == null)
            {
                throw new ArgumentException("TournamentId null on update for Tournament");
            }
            if (tournament.Creator?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId null on update for Tournament");
            }
            using (DbCommand command = CreateUpdateCmd(tournament.TournamentId.Value, 
                tournament.Name, tournament.Creator.PlayerId.Value))
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
            using (DbCommand command = CreateCountCmd())
            {
                return database.ExecuteScalar(command);
            }
        }
    }
}
