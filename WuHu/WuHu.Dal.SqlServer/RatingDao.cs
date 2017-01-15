using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.SqlServer
{
    public class RatingDao : IRatingDao
    {
        private const string SqlFindById =
          @"SELECT * 
            FROM Rating JOIN Player on (Rating.playerId = Player.playerId)
            WHERE ratingId = @ratingId;";

        private const string SqlFindAll =
            @"SELECT * 
                FROM Rating JOIN Player on (Rating.playerId = Player.playerId)
                ORDER BY value DESC;";

        private const string SqlFindAllByPlayer =
            @"SELECT * 
                FROM Rating JOIN Player on (Rating.playerId = Player.playerId)
                WHERE Player.playerId = @playerId;";

        private const string SqlFindCurrentRating =
            @"SELECT *
                FROM Rating rOuter JOIN Player p on (rOuter.playerId = p.playerId)
                WHERE p.playerId = @playerId
                    AND rOuter.datetime = 
                    (SELECT MAX(datetime)
                        FROM Rating rInner 
                        WHERE rInner.playerId = p.playerId);";

        private const string SqlInsert =
            @"INSERT INTO Rating (playerId, datetime, value)
                OUTPUT Inserted.ratingId
                VALUES (@playerId, @datetime, @value);";

        private const string SqlCount =
            @"SELECT Count(*) FROM Rating;";

        private const string SqlUpdate =
            @"UPDATE Rating
                SET playerId = @playerId,
                    datetime = @datetime,
                    value = @value
                WHERE ratingId = @ratingId";

        private readonly IDatabase database;

        public RatingDao(IDatabase database)
        {
            this.database = database;
        }

        protected DbCommand CreateFindAllCmd()
        {
            return database.CreateCommand(SqlFindAll);
        }

        public IList<Rating> FindAll()
        {
            using (var command = CreateFindAllCmd())
            using (var reader = database.ExecuteReader(command))
            {
                var result = new List<Rating>();
                while (reader.Read())
                    result.Add(new Rating((int)reader["ratingId"],
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
                                                null : (byte[])reader["picture"]),
                                          (DateTime)reader["datetime"],
                                          (int)reader["value"]));
                return result;
            }
        }

        public DbCommand CreateFindAllByPlayerCmd(int playerId)
        {
            var command = database.CreateCommand(SqlFindAllByPlayer);
            database.DefineParameter(command, "playerId", DbType.Int32, playerId);
            return command;
        }

        public IList<Rating> FindAllByPlayer(Player player)
        {
            if (player?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId null on update");
            }
            using (var command = CreateFindAllByPlayerCmd(player.PlayerId.Value))
            using (var reader = database.ExecuteReader(command))
            {
                var result = new List<Rating>();
                while (reader.Read())
                    result.Add(new Rating((int)reader["ratingId"],
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
                                                null : (byte[])reader["picture"]),
                                          (DateTime)reader["datetime"],
                                          (int)reader["value"]));
                return result;
            }
        }

        protected DbCommand CreateFindCurrentRatingCommand(int playerId)
        {
            var command = database.CreateCommand(SqlFindCurrentRating);
            database.DefineParameter(command, "playerId", DbType.Int32, playerId);
            return command;
        }

        public Rating FindCurrentRating(Player player)
        {
            if (player?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId null");
            }
            return FindCurrentRating(player.PlayerId.Value);
        }

        public Rating FindCurrentRating(int playerId)
        {
            using (var command = CreateFindCurrentRatingCommand(playerId))
            using (var reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Rating((int)reader["ratingId"],
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
                                                null : (byte[])reader["picture"]),
                                          (DateTime)reader["datetime"],
                                          (int)reader["value"]);
                }
                else
                {
                    return null;
                }
            }
        }

        protected DbCommand CreateFindByIdCmd(int ratingId)
        {
            var findByIdCmd = database.CreateCommand(SqlFindById);
            database.DefineParameter(findByIdCmd, "ratingId", DbType.Int32, ratingId);
            return findByIdCmd;
        }

        public Rating FindById(int ratingId)
        {
            using (var command = CreateFindByIdCmd(ratingId))
            using (var reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Rating((int)reader["ratingId"],
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
                                                null : (byte[])reader["picture"]),
                                      (DateTime)reader["datetime"],
                                      (int)reader["value"]);
                }
                else
                {
                    return null;
                }
            }
        }

        private DbCommand CreateInsertCmd(int playerId, DateTime datetime, int value)
        {
            var cmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(cmd, "playerId", DbType.Int32, playerId);
            database.DefineParameter(cmd, "datetime", DbType.DateTime2, datetime);
            database.DefineParameter(cmd, "value", DbType.Int32, value);
            return cmd;
        }

        public bool Insert(Rating rating)
        {
            if (rating.Player?.PlayerId == null)
            {
                throw new ArgumentException("No playerId for Insert into Rating provided.");
            }

            using (var command = CreateInsertCmd(rating.Player.PlayerId.Value, rating.Datetime, rating.Value))
            {
                var id = database.ExecuteScalar(command); // set the objects id right away
                rating.RatingId = id;
                return true;
            }
        }

        // Update
        protected DbCommand CreateUpdateCmd(int ratingId, int playerId, DateTime datetime, int value)
        {
            var updateByIdCmd = database.CreateCommand(SqlUpdate);
            database.DefineParameter(updateByIdCmd, "ratingId", DbType.Int32, ratingId);
            database.DefineParameter(updateByIdCmd, "playerId", DbType.Int32, playerId);
            database.DefineParameter(updateByIdCmd, "datetime", DbType.DateTime, datetime);
            database.DefineParameter(updateByIdCmd, "value", DbType.Int32, value);

            return updateByIdCmd;
        }

        public bool Update(Rating rating)
        {
            if (rating.RatingId == null)
            {
                throw new ArgumentException("RatingId null on update for Rating");
            }
            if (rating.Player?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId null on update for Rating");
            }
            using (var command = CreateUpdateCmd(rating.RatingId.Value,
                rating.Player.PlayerId.Value, rating.Datetime, rating.Value))
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
