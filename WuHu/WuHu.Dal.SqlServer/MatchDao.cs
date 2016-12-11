using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.SqlServer
{
    public class MatchDao : IMatchDao
    {
        private const string SqlFindById =
          @"SELECT *
            FROM Match m
            WHERE MatchId = @MatchId;";

        private const string SqlFindAll =
            @"SELECT *
            FROM Match
                ORDER BY datetime DESC;";

        private const string SqlFindAllByPlayer =
            @"SELECT *
            FROM Match m
                WHERE player1 = @playerId
                    OR player2 = @playerId
                    OR player3 = @playerId
                    OR player4 = @playerId
                ORDER BY datetime DESC;";

        private const string SqlFindAllByTournament =
            @"SELECT *
            FROM Match
                WHERE tournamentId = @tournamentId;";

        private const string SqlInsert =
            @"INSERT INTO Match (tournamentId, datetime, scoreTeam1, scoreTeam2,
                                estimatedWinChance, isDone, player1, player2, player3, player4)
                OUTPUT Inserted.MatchId
                VALUES (@tournamentId, @datetime, @scoreTeam1, @scoreTeam2,
                        @estimatedWinChance, @isDone, @player1, @player2, @player3, @player4);";

        private const string SqlCount =
            @"SELECT Count(*) 
                FROM Match;";

        private const string SqlUpdate =
            @"UPDATE Match
                SET tournamentId = @tournamentId, 
                    datetime = @datetime, 
                    scoreTeam1 = @scoreTeam1, 
                    scoreTeam2 = @scoreTeam2,
                    estimatedWinChance = @estimatedWinChance, 
                    isDone = @isDone, 
                    player1 = @player1, 
                    player2 = @player2, 
                    player3 = @player3, 
                    player4 = @player4
                WHERE MatchId = @MatchId";

        private const string SqlDelete =
            @"DELETE FROM Match
                WHERE MatchId = @MatchId";


        private readonly IDatabase database;

        public MatchDao(IDatabase database)
        {
            this.database = database;
        }


        protected Match BuildMatch(IDataReader reader)
        {
            IPlayerDao playerDao = DalFactory.CreatePlayerDao(database);

            Tournament tournament = DalFactory.CreateTournamentDao(database)
                .FindById((int)reader["tournamentId"]);
            
            Player p1 = playerDao.FindById((int)reader["player1"]);
            Player p2 = playerDao.FindById((int)reader["player2"]);
            Player p3 = playerDao.FindById((int)reader["player3"]);
            Player p4 = playerDao.FindById((int)reader["player4"]);

            return new Match((int)reader["matchId"],
                tournament,
               (DateTime)reader["datetime"],
               reader.IsDBNull(reader.GetOrdinal("scoreTeam1")) ? 
                   null : 
                   (byte?)reader["scoreTeam1"],
               reader.IsDBNull(reader.GetOrdinal("scoreTeam2")) ?
                   null :
                   (byte?)reader["scoreTeam2"],
               (double)reader["estimatedWinChance"],
               (bool)reader["isDone"],
               p1,
               p2,
               p3,
               p4);
        }

        protected DbCommand CreateFindAllCmd()
        {
            return database.CreateCommand(SqlFindAll);
        }

        public IList<Match> FindAll()
        {
            using (DbCommand command = CreateFindAllCmd())
            using (IDataReader reader = database.ExecuteReader(command))
            {
                var result = new List<Match>();
                while (reader.Read())
                {
                    result.Add(BuildMatch(reader));
                }
                return result;
            }
        }

        protected DbCommand CreateFindAllByPlayerCmd(int playerId)
        {
            DbCommand findByIdCmd = database.CreateCommand(SqlFindAllByPlayer);
            database.DefineParameter(findByIdCmd, "playerId", DbType.Int32, playerId);
            return findByIdCmd;
        }

        public IList<Match> FindAllByPlayer(Player player)
        {
            if (player?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId null for finding Matches");
            }
            using (DbCommand command = CreateFindAllByPlayerCmd(player.PlayerId.Value))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                var result = new List<Match>();
                while (reader.Read())
                {
                    result.Add(BuildMatch(reader));
                }
                return result;
            }
        }

        protected DbCommand CreateFindAllByTournamentCmd(int tournamentId)
        {
            DbCommand findByIdCmd = database.CreateCommand(SqlFindAllByTournament);
            database.DefineParameter(findByIdCmd, "tournamentId", DbType.Int32, tournamentId);
            return findByIdCmd;
        }

        public IList<Match> FindAllByTournament(Tournament tournament)
        {
            if (tournament?.TournamentId == null)
            {
                throw new ArgumentException("TournamentId Null for finding Matches");
            }
            using (DbCommand command = CreateFindAllByTournamentCmd(tournament.TournamentId.Value))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                var result = new List<Match>();
                while (reader.Read())
                {
                    result.Add(BuildMatch(reader));
                }
                return result;
            }
        }

        protected DbCommand CreateFindByIdCmd(int matchId)
        {
            DbCommand findByIdCmd = database.CreateCommand(SqlFindById);
            database.DefineParameter(findByIdCmd, "matchId", DbType.Int32, matchId);
            return findByIdCmd;
        }


        public Match FindById(int matchId)
        {
            using (DbCommand command = CreateFindByIdCmd(matchId))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return BuildMatch(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        
        private DbCommand CreateInsertCmd(int tournamentId, DateTime datetime, 
            byte? scoreTeam1, byte? scoreTeam2, double estimatedWinChance, 
            bool isDone, int player1, int player2, int player3, int player4)
        {
            DbCommand cmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(cmd, "tournamentId", DbType.Int32, tournamentId);
            database.DefineParameter(cmd, "player1", DbType.Int32, player1);
            database.DefineParameter(cmd, "player2", DbType.Int32, player2);
            database.DefineParameter(cmd, "player3", DbType.Int32, player3);
            database.DefineParameter(cmd, "player4", DbType.Int32, player4);
            database.DefineParameter(cmd, "datetime", DbType.DateTime2, datetime);
            database.DefineParameter(cmd, "scoreTeam1", DbType.Byte, scoreTeam1 ?? SqlByte.Null);
            database.DefineParameter(cmd, "scoreTeam2", DbType.Byte, scoreTeam2 ?? SqlByte.Null);
            database.DefineParameter(cmd, "isDone", DbType.Boolean, isDone);
            database.DefineParameter(cmd, "estimatedWinChance", DbType.Double, estimatedWinChance);
            return cmd;
        }

        public bool Insert(Match match)
        {
            if (match.Player1?.PlayerId == null || match.Player2?.PlayerId == null ||
                match.Player3?.PlayerId == null || match.Player4?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId for Insert into Match missing.");
            }
            if (match.Tournament?.TournamentId == null)
            {
                throw new ArgumentException("TournamentId for Insert into Match missing.");
            }

            using (DbCommand command = CreateInsertCmd(match.Tournament.TournamentId.Value, 
                match.Datetime, match.ScoreTeam1, match.ScoreTeam2, match.EstimatedWinChance,
                match.IsDone, match.Player1.PlayerId.Value, match.Player2.PlayerId.Value, 
                match.Player3.PlayerId.Value, match.Player4.PlayerId.Value))
            {
                try
                {
                    var id = database.ExecuteScalar(command); // set the objects id right away
                    match.MatchId = id;
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
        }

        private DbCommand CreateUpdateCmd(int matchId, int tournamentId, DateTime datetime,
            byte? scoreTeam1, byte? scoreTeam2, double estimatedWinChance,
            bool isDone, int player1, int player2, int player3, int player4)
        {
            DbCommand cmd = database.CreateCommand(SqlUpdate);
            database.DefineParameter(cmd, "matchId", DbType.Int32, matchId);
            database.DefineParameter(cmd, "tournamentId", DbType.Int32, tournamentId);
            database.DefineParameter(cmd, "player1", DbType.Int32, player1);
            database.DefineParameter(cmd, "player2", DbType.Int32, player2);
            database.DefineParameter(cmd, "player3", DbType.Int32, player3);
            database.DefineParameter(cmd, "player4", DbType.Int32, player4);
            database.DefineParameter(cmd, "datetime", DbType.DateTime2, datetime);
            database.DefineParameter(cmd, "scoreTeam1", DbType.Int32, scoreTeam1);
            database.DefineParameter(cmd, "scoreTeam2", DbType.Int32, scoreTeam2);
            database.DefineParameter(cmd, "isDone", DbType.Boolean, isDone);
            database.DefineParameter(cmd, "estimatedWinChance", DbType.Double, estimatedWinChance);
            return cmd;
        }

        public bool Update(Match match)
        {
            if (match.Player1?.PlayerId == null || match.Player2?.PlayerId == null ||
                match.Player3?.PlayerId == null || match.Player4?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId for Update on Match missing.");
            }
            if (match.Tournament?.TournamentId == null)
            {
                throw new ArgumentException("TournamentId for Update on Match missing.");
            }
            if (match.MatchId == null)
            {
                throw new ArgumentException("MatchId for Update on Match missing.");
            }

            using (DbCommand command = CreateUpdateCmd(match.MatchId.Value, match.Tournament.TournamentId.Value,
                match.Datetime, match.ScoreTeam1, match.ScoreTeam2, match.EstimatedWinChance,
                match.IsDone, match.Player1.PlayerId.Value, match.Player2.PlayerId.Value,
                match.Player3.PlayerId.Value, match.Player4.PlayerId.Value))
            {
                return database.ExecuteNonQuery(command) == 1; 
            }
        }

        protected DbCommand CreateDeleteCmd(int matchId)
        {
            DbCommand cmd = database.CreateCommand(SqlDelete);
            database.DefineParameter(cmd, "matchId", DbType.Int32, matchId);
            return cmd;
        }

        public bool Delete(Match match)
        {
            if (match.MatchId == null)
            {
                throw new ArgumentException("MatchId for Delete on Match missing.");
            }

            using (DbCommand command = CreateDeleteCmd(match.MatchId.Value))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        protected DbCommand CreateCountCmd()
        {
            DbCommand countCmd = database.CreateCommand(SqlCount);
            return countCmd;
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
