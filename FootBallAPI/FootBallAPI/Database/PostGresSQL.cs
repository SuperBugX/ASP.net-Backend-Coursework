using FootballAPI.Configuration;
using FootballAPI.Interfaces.Database;
using Npgsql;
using Polly.CircuitBreaker;
using System.Data.Common;

namespace FootballAPI.Database
{
    public class PostGresSQL : IDatabase
    {
        //Attributes
        public string ConnectionString { get; set; }
        public string PoolingConfig { get; set; }
        private readonly AsyncCircuitBreakerPolicy _circuitPolicy;

        //Methods
        public PostGresSQL(IConfiguration config)
        {
            ConnectionString = config["Database:ConnectionString"];
            PoolingConfig = config["Database:PoolingConfig"];
            _circuitPolicy = PollyPoliciesConfig.Instance.PostGresSqlCircuitPolicy;
        }

        public PostGresSQL(string connectionString)
        {
            ConnectionString = connectionString;
            PoolingConfig = "Pooling=true;Minimum Pool Size=1;Maximum Pool Size=4;";
            _circuitPolicy = PollyPoliciesConfig.Instance.PostGresSqlCircuitPolicy;
        }

        public PostGresSQL(string connectionString, string poolingConfig)
        {
            ConnectionString = connectionString;
            PoolingConfig = poolingConfig;
            _circuitPolicy = PollyPoliciesConfig.Instance.PostGresSqlCircuitPolicy;
        }

        //Method returns if the provided connection object has an active open connection
        private Boolean IsConnected(NpgsqlConnection connection)
        {
            return connection.State == System.Data.ConnectionState.Open;
        }

        //Method when provided with a NpgsqlConnection connection object attempts to open a connection
        //Note:A NoConnectionException is thrown on failure.
        private void Connect(NpgsqlConnection connection)
        {
            if (connection != null && !IsConnected(connection))
            {
                connection.Open();
            }
        }

        //Method when provided with a connection attempts to close the connection
        private void Disconnect(NpgsqlConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        /*
            Method performs a "ExecuteNonQuery" when provided with a string representing an SQL statement.
            On success, the method returns the number of rows affected, -1 if none have been modified.
            
            NOTE:A SqlException is thrown on failure.
        */
        public long ExecuteNonQuery(string sql)
        {
            return _circuitPolicy.ExecuteAsync(async () =>
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString + PoolingConfig))
                {

                    Connect(connection);
                    var cmd = new NpgsqlCommand(sql, connection);
                    //Execute NonQuery and return result
                    return (long)cmd.ExecuteNonQuery();
                }
            }).Result;
        }

        /*
            Method performs a "ExecuteReader" when provided with a string representing an SQL statement
            and a function delegate that when provided with a DbDataReader, parses the rows retrieved 
            and returns a an object result of type T.

            NOTE:A SqlException is thrown on failure.
        */
        public T ExecuteDataReader<T>(string sql, Func<DbDataReader, T> readerFunc) where T : class
        {
            return _circuitPolicy.ExecuteAsync(async () =>
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString + PoolingConfig))
                {
                    Connect(connection);
                    var cmd = new NpgsqlCommand(sql, connection);
                    //Execute Reader method and acquire reader
                    DbDataReader reader = cmd.ExecuteReader();
                    //Provide reader to delegate function and return function result
                    return readerFunc(reader);
                }
            }).Result;
        }

        /*
            Method performs a "ExecutePreparedStatement" when provided with a string representing an SQL statement
            and a Enumerable containing KeyValuePair objects. Each key and value pair must match with a 
            placeholder value (based on key) to be replaced with a value in the provided string sql argument.
            On success, the method returns the first column of the first row value, if empty, -1 is returned.

            NOTE:A SqlException is thrown on failure.
        */
        public long ExecutePreparedStatement(string sql, IEnumerable<KeyValuePair<string, Object>> values)
        {
            return _circuitPolicy.ExecuteAsync(async () =>
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString + PoolingConfig))
                {

                    Connect(connection);
                    var cmd = new NpgsqlCommand(sql, connection);

                    //Substitute placeholder values in sql with values based on
                    //the key of the KeyValuePair Enumerable
                    foreach (var pair in values)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    //Prepare statement and return query result
                    cmd.Prepare();
                    return (long)(cmd.ExecuteScalar() ?? (long)-1);

                }
            }).Result;
        }
    }
}
