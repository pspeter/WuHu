using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using System.Transactions;
using System.Data.SqlClient;
using System.IO;

namespace WuHu.Dal.SqlServer
{

    public class Database : IDatabase
    {
        private readonly string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbCommand CreateCommand(string commandText)
        {
            return new SqlCommand(commandText);
        }

        public int DeclareParameter(DbCommand command, string name, DbType type)
        {
            if (!command.Parameters.Contains(name))
            {
                return command.Parameters.Add(new SqlParameter(name, type));
            }
            else
            {
                throw new ArgumentException($"Parameter {name} already exists.");
            }
        }

        public void SetParameter(DbCommand command, string name, object value)
        {
            if (command.Parameters.Contains(name))
            {
                command.Parameters[name].Value = value;
            }
            else
            {
                throw new ArgumentException($"Parameter {name} is not declared.");
            }
        }

        public void DefineParameter(DbCommand command, string name, DbType type, object value)
        {
            int index = DeclareParameter(command, name, type);
            command.Parameters[index].Value = value;
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            DbConnection connection = null;

            try
            {
                connection = GetOpenConnection();
                command.Connection = connection;
                return command.ExecuteNonQuery();
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        public IDataReader ExecuteReader(DbCommand command)
        {
            DbConnection connection = null;
            try
            {
                connection = GetOpenConnection();
                command.Connection = connection;
                var behavior = Transaction.Current == null ?
                    CommandBehavior.CloseConnection : CommandBehavior.Default;
                
                return command.ExecuteReader(behavior);
            }
            catch
            {
                ReleaseConnection(connection);
                throw;
            }

            // we don't use finally here because the reader still needs the 
            // connection. It will be closed by the reader's Dispose method.
        }

        public int ExecuteScalar(DbCommand command)
        {
            DbConnection connection = null;

            try
            {
                connection = GetOpenConnection();
                command.Connection = connection;
                return (int) command.ExecuteScalar();
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }
        
        [ThreadStatic] // one instance for every thread, not only one for all threads
        private static DbConnection sharedConnection;

        private DbConnection CreateOpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            sharedConnection = connection;
            return connection;
        }

        private DbConnection GetOpenConnection()
        {
            Transaction currentTransaction = Transaction.Current;

            if (currentTransaction == null)
            {
                return CreateOpenConnection();
            }
            else
            {
                if (sharedConnection == null)
                {
                    sharedConnection = CreateOpenConnection();
                    currentTransaction.TransactionCompleted += (s, e) =>
                    {
                        sharedConnection.Close();
                        sharedConnection = null;
                    };
                }
                return sharedConnection;
            }
        }

        private void ReleaseConnection(DbConnection connection)
        {
            if (connection != null && Transaction.Current == null)
            {
                connection?.Close();
            }
        }
    }

}
