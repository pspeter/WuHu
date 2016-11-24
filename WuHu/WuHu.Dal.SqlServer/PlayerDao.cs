using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;
using System.Security.Cryptography;

namespace WuHu.Dal.SqlServer
{
    class PlayerDao : IPlayerDao
    {
        private const string SqlFindByString =
            @"SELECT * FROM Player 
                WHERE firstName LIKE @name
                    OR lastName LIKE @name
                    OR nickName LIKE @name
                    OR userName LIKE @name;";

        private const string SqlFindById =
          @"SELECT * FROM Player 
            WHERE playerId = @playerId;";

        private const string SqlFindAll = @"SELECT * FROM Player;";

        private const string SqlFindAllOnDays = @"SELECT * FROM Player 
                                                WHERE playsMondays = @playsMondays OR playsMondays = 1 AND
                                                      playsTuesdays = @playsTuesdays OR playsTuesdays = 1 AND
                                                      playsWednesdays = @playsWednesdays OR playsWednesdays = 1 AND
                                                      playsThursdays = @playsThursdays OR playsThursdays = 1 AND
                                                      playsFridays = @playsFridays OR playsFridays = 1 AND
                                                      playsSaturdays = @playsSaturdays OR playsSaturdays = 1 AND
                                                      playsSundays = @playsSundays OR playsSundays = 1;";

        private const string SqlUpdateById =
          @"UPDATE Player
            SET firstName = @firstName, 
                lastName = @lastName,
                nickName = @nickName,
                userName = @userName,
                password = @password,
                salt = @salt,
                isAdmin = @isAdmin,
                playsMondays = @playsMondays,
                playsTuesdays = @playsTuesdays,
                playsWednesdays = @playsWednesdays,
                playsThursdays = @playsThursdays,
                playsFridays = @playsFridays,
                playsSaturdays = @playsSaturdays,
                playsSundays = @playsSundays,
                picture = @picture
            WHERE playerId = @playerId;";

        private const string SqlInsert =
          @"INSERT INTO Player (firstName,lastName,nickName,userName,password,salt,isAdmin,
                    playsMondays,playsTuesdays,playsWednesdays,playsThursdays,playsFridays,playsSaturdays,playsSundays,picture)
            OUTPUT Inserted.playerId
            VALUES (@firstName, @lastName, @nickName, @userName, @password, @salt, @isAdmin, @playsMondays, 
                    @playsTuesdays, @playsWednesdays, @playsThursdays, @playsFridays, @playsSaturdays, @playsSundays, @picture);";

        private const string SqlCount =
            @"SELECT Count(*) as Cnt FROM Player;";

        private readonly IDatabase database;

        public PlayerDao(IDatabase database)
        {
            this.database = database;
        }



        protected DbCommand CreateFindAllCmd()
        {
            return database.CreateCommand(SqlFindAll);
        }


        public IList<Player> FindAll()
        {
            using (DbCommand command = CreateFindAllCmd())
            using (IDataReader reader = database.ExecuteReader(command))
            {
                IList<Player> result = new List<Player>();
                while (reader.Read())
                    result.Add(new Player((int) reader["playerId"], 
                                          (string) reader["firstName"],
                                          (string) reader["lastName"],
                                          (string) reader["nickName"],
                                          (string) reader["userName"],
                                          (byte[]) reader["password"],
                                          (byte[]) reader["salt"],
                                          (bool) reader["isAdmin"],
                                          (bool) reader["playsMondays"],
                                          (bool) reader["playsTuesdays"],
                                          (bool) reader["playsWednesdays"],
                                          (bool) reader["playsThursdays"],
                                          (bool) reader["playsFridays"],
                                          (bool) reader["playsSaturdays"],
                                          (bool) reader["playsSundays"],
                                          reader.IsDBNull(reader.GetOrdinal("picture")) ?
                                            null : (byte[])reader["picture"]));
                return result;
            }
        }

        private DbCommand CreateFindAllOnDaysCmd(bool playsMondays = false, 
            bool playsTuesdays = false, bool playsWednesdays = false,
            bool playsThursdays = false, bool playsFridays = false, bool playsSaturdays = false,
            bool playsSundays = false)
        {
            DbCommand findCmd = database.CreateCommand(SqlFindAllOnDays);
            database.DefineParameter(findCmd, "playsMondays", DbType.Boolean, playsMondays);
            database.DefineParameter(findCmd, "playsTuesdays", DbType.Boolean, playsTuesdays);
            database.DefineParameter(findCmd, "playsWednesdays", DbType.Boolean, playsWednesdays);
            database.DefineParameter(findCmd, "playsThursdays", DbType.Boolean, playsThursdays);
            database.DefineParameter(findCmd, "playsFridays", DbType.Boolean, playsFridays);
            database.DefineParameter(findCmd, "playsSaturdays", DbType.Boolean, playsSaturdays);
            database.DefineParameter(findCmd, "playsSundays", DbType.Boolean, playsSundays);

            return findCmd;
        }

        public IList<Player> FindAllOnDays(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            using (DbCommand command = CreateFindAllOnDaysCmd(monday, tuesday, wednesday, thursday, friday, saturday, sunday))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                IList<Player> result = new List<Player>();
                while (reader.Read())
                    result.Add(new Player((int)reader["playerId"],
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
                return result;
            }
        }

        protected DbCommand CreateFindByIdCmd(int playerId)
        {
            DbCommand findByIdCmd = database.CreateCommand(SqlFindById);
            database.DefineParameter(findByIdCmd, "playerId", DbType.Int32, playerId);
            return findByIdCmd;
        }

        public Player FindById(int playerId)
        {
            using (DbCommand command = CreateFindByIdCmd(playerId))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Player((int)reader["playerId"],
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
                                        null : (byte[])reader["picture"]);
                }
                else
                {
                    return null;
                }
            }
        }

        private DbCommand CreateFindAllByStringCmd(string name)
        {
            DbCommand findCmd = database.CreateCommand(SqlFindByString);
            database.DefineParameter(findCmd, "name", DbType.String, "%" + name + "%");
            return findCmd;
        }

        public IList<Player> FindAllByString(string name)
        {
            using (DbCommand command = CreateFindAllByStringCmd(name))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                IList<Player> result = new List<Player>();
                while (reader.Read())
                    result.Add(new Player((int)reader["playerId"],
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
                return result;
            }
        }


        protected DbCommand CreateInsertCmd(string firstName, string lastName, string nickName,
                                            string userName, byte[] password, byte[] salt, bool isAdmin,
                                            bool playsMondays, bool playsTuesdays, bool playsWednesdays,
                                            bool playsThursdays, bool playsFridays, bool playsSaturdays,
                                            bool playsSundays, byte[] picture)
        {
            DbCommand insertCmd = database.CreateCommand(SqlInsert);
            database.DefineParameter(insertCmd, "firstName", DbType.String, firstName);
            database.DefineParameter(insertCmd, "lastName", DbType.String, lastName);
            database.DefineParameter(insertCmd, "nickName", DbType.String, nickName);
            database.DefineParameter(insertCmd, "userName", DbType.String, userName);
            database.DefineParameter(insertCmd, "password", DbType.Binary, password);
            database.DefineParameter(insertCmd, "salt", DbType.Binary, salt);
            database.DefineParameter(insertCmd, "isAdmin", DbType.Boolean, isAdmin);
            database.DefineParameter(insertCmd, "playsMondays", DbType.Boolean, playsMondays);
            database.DefineParameter(insertCmd, "playsTuesdays", DbType.Boolean, playsTuesdays);
            database.DefineParameter(insertCmd, "playsWednesdays", DbType.Boolean, playsWednesdays);
            database.DefineParameter(insertCmd, "playsThursdays", DbType.Boolean, playsThursdays);
            database.DefineParameter(insertCmd, "playsFridays", DbType.Boolean, playsFridays);
            database.DefineParameter(insertCmd, "playsSaturdays", DbType.Boolean, playsSaturdays);
            database.DefineParameter(insertCmd, "playsSundays", DbType.Boolean, playsSundays);
            database.DefineParameter(insertCmd, "picture", DbType.Binary, picture ?? SqlBinary.Null);
            return insertCmd;
        }
        

        public int Insert(Player player)
        {
            using (DbCommand command = CreateInsertCmd(player.FirstName, player.LastName, player.NickName,
                                                        player.UserName, player.Password, player.Salt, player.IsAdmin,
                                                        player.PlaysMondays, player.PlaysTuesdays, player.PlaysWednesdays,
                                                        player.PlaysThursdays, player.PlaysFridays, player.PlaysSaturdays,
                                                        player.PlaysSundays, player.Picture))
            {
                int playerId = database.ExecuteScalar(command);
                player.PlayerId = playerId;
                return playerId;
            }
        }


        // Update
        protected DbCommand CreateUpdateByIdCmd(int playerId, string firstName, string lastName, string nickName,
                                            string userName, byte[] password, byte[] salt, bool isAdmin,
                                            bool playsMondays, bool playsTuesdays, bool playsWednesdays,
                                            bool playsThursdays, bool playsFridays, bool playsSaturdays,
                                            bool playsSundays, byte[] picture)
        {
            DbCommand updateByIdCmd = database.CreateCommand(SqlUpdateById);
            database.DefineParameter(updateByIdCmd, "playerId", DbType.Int32, playerId);
            database.DefineParameter(updateByIdCmd, "firstName", DbType.String, firstName);
            database.DefineParameter(updateByIdCmd, "lastName", DbType.String, lastName);
            database.DefineParameter(updateByIdCmd, "nickName", DbType.String, nickName);
            database.DefineParameter(updateByIdCmd, "userName", DbType.String, userName);
            database.DefineParameter(updateByIdCmd, "password", DbType.Binary, password);
            database.DefineParameter(updateByIdCmd, "salt", DbType.Binary, salt);
            database.DefineParameter(updateByIdCmd, "isAdmin", DbType.Boolean, isAdmin);
            database.DefineParameter(updateByIdCmd, "playsMondays", DbType.Boolean, playsMondays);
            database.DefineParameter(updateByIdCmd, "playsTuesdays", DbType.Boolean, playsTuesdays);
            database.DefineParameter(updateByIdCmd, "playsWednesdays", DbType.Boolean, playsWednesdays);
            database.DefineParameter(updateByIdCmd, "playsThursdays", DbType.Boolean, playsThursdays);
            database.DefineParameter(updateByIdCmd, "playsFridays", DbType.Boolean, playsFridays);
            database.DefineParameter(updateByIdCmd, "playsSaturdays", DbType.Boolean, playsSaturdays);
            database.DefineParameter(updateByIdCmd, "playsSundays", DbType.Boolean, playsSundays);
            database.DefineParameter(updateByIdCmd, "picture", DbType.Binary, picture ?? SqlBinary.Null);

            return updateByIdCmd;
        }

        public bool Update(Player player)
        {
            if (player?.PlayerId == null)
            {
                throw new ArgumentException("PlayerId null on update");
            }
            using (DbCommand command = CreateUpdateByIdCmd(player.PlayerId.Value, player.FirstName, player.LastName, player.NickName,
                                            player.UserName, player.Password, player.Salt, player.IsAdmin,
                                            player.PlaysMondays, player.PlaysTuesdays, player.PlaysWednesdays,
                                            player.PlaysThursdays, player.PlaysFridays, player.PlaysSaturdays,
                                            player.PlaysSundays, player.Picture))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }


        // Count

        protected DbCommand CreateCountCmd()
        {
            DbCommand countCmd = database.CreateCommand(SqlCount);
            return countCmd;
        }

        public int Count()
        {
            using (DbCommand command = CreateCountCmd())
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return (int)reader["Cnt"];
                }
                return 0;
            }
        }
    }
}
