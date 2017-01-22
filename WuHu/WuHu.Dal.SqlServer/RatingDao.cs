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
        private const int PageSize = 2000;

        private const string SqlFindById =
          @"SELECT * 
            FROM Rating JOIN Player on (Rating.playerId = Player.playerId)
            WHERE ratingId = @ratingId;";

        private const string SqlFindAll =
            @"SELECT * 
                FROM Rating JOIN Player on (Rating.playerId = Player.playerId)
                ORDER BY datetime DESC
                OFFSET @offset ROWS
                FETCH NEXT @fetch ROWS ONLY;";

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
                        WHERE rInner.playerId = @playerId);";

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

        private Rating BuildVirtualMatch(IDataReader reader)
        {
            var p = new Player()
            {
                PlayerId = (int) reader["playerId"]
            };

            return new Rating((int) reader["ratingId"], p, (DateTime) reader["datetime"], (int) reader["value"]);
        }

        public RatingDao(IDatabase database)
        {
            this.database = database;
        }

        protected DbCommand CreateFindAllCmd(int page)
        {
            var cmd = database.CreateCommand(SqlFindAll);
            var offset = PageSize * page;
            database.DefineParameter(cmd, "offset", DbType.Int32, offset);
            database.DefineParameter(cmd, "fetch", DbType.Int32, PageSize);
            return cmd;
        }

        public IList<Rating> FindAll(int page)
        {
            using (var command = CreateFindAllCmd(page))
            using (var reader = database.ExecuteReader(command))
            {
                var result = new List<Rating>();
                while (reader.Read())
                    result.Add(BuildVirtualMatch(reader));
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
                    result.Add(BuildVirtualMatch(reader));
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
                    return BuildVirtualMatch(reader);
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
                    return BuildVirtualMatch(reader);
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
        public int PageCount()
        {
            using (var command = CreateCountCmd())
            {
                return (database.ExecuteScalar(command) / PageSize) + 1;
            }
        }
    }
}
