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
    class RatingDao : IRatingDao
    {
        private const string SqlFindById =
          @"SELECT * 
            FROM Rating JOIN Player on (Rating.playerId = Player.playerId)
            WHERE ratingId = @ratingId;";

        private const string SqlFindAll = @"SELECT * FROM Rating JOIN Player on (Rating.playerId = Player.playerId);";

        private const string SqlInsert =
            @"INSERT INTO Rating (playerId, datetime, value)
                OUTPUT Inserted.ratingId
                VALUES (@playerId, @datetime, @value);";

        private const string SqlCount =
            @"SELECT Count(*) FROM Rating JOIN Player on (Rating.playerId = Player.playerId);";

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
            using (DbCommand command = CreateFindAllCmd())
            using (IDataReader reader = database.ExecuteReader(command))
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

        public IList<Rating> FindAllByPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public Rating FindById(int ratingId)
        {
            throw new NotImplementedException();
        }

        private DbCommand CreateInsertCmd(int playerId, DateTime datetime, int value)
        {
            DbCommand cmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(cmd, "playerId", DbType.Int32, playerId);
            database.DefineParameter(cmd, "datetime", DbType.DateTime2, datetime);
            database.DefineParameter(cmd, "value", DbType.Int32, value);
            return cmd;
        }

        public int Insert(Rating rating)
        {
            if (rating.Player?.PlayerId == null)
            {
                throw new ArgumentException("No playerId for Insert into Rating provided.");
            }

            using (DbCommand command = CreateInsertCmd(rating.Player.PlayerId.Value, rating.Datetime, rating.Value))
            {
                return database.ExecuteScalar(command);
            }
        }

        public bool Update(Rating rating)
        {
            throw new NotImplementedException();
        }

        private DbCommand CreateCountCmd()
        {
            return database.CreateCommand(SqlCount);
        }
        public int Count()
        {
            using(DbCommand command = CreateCountCmd())
            {
                return database.ExecuteScalar(command);
            }
        }
    }
}
