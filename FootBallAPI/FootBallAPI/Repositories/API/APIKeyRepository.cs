using FootballAPI.Interfaces.Database;
using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Models.API;
using System.Data.Common;

namespace FootballAPI.Repositories.API
{
    public class APIKeyRepository : IRepository<APIKeySubscription>
    {
        //Attributes
        private static IDatabase _database;

        //Methods
        public APIKeyRepository(IDatabase database)
        {
            _database = database;
        }

        public LinkedList<APIKeySubscription> All()
        {
            //Select SQL Query
            string getSQL = "SELECT id, key, role FROM exercise.api_keys JOIN exercise.api_key_types ON api_key_type_fk = exercise.api_key_types.id";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<APIKeySubscription>> readerFunction = (reader) =>
            {
                LinkedList<APIKeySubscription> apiKeySubscriptions = new LinkedList<APIKeySubscription>();

                //Loop records and create objects
                while (reader.Read())
                {
                    APIKeySubscription apiKeySubscription = new APIKeySubscription(Convert.ToInt64(reader["id"]), Convert.ToString(reader["key"]), Convert.ToString(reader["role"]));
                    apiKeySubscriptions.AddLast(apiKeySubscription);
                }

                return apiKeySubscriptions;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public APIKeySubscription? Get(long id)
        {
            //Select SQL Query
            string getSQL = "SELECT key, role FROM exercise.api_keys JOIN exercise.api_key_types ON api_key_type_fk = exercise.api_key_types.id WHERE exercise.api_keys.id='" + id + "';";

            //Delegate function to read result rows
            Func<DbDataReader, APIKeySubscription> readerFunction = (reader) =>
            {
                APIKeySubscription apiKeySubscription = null;

                //Loop records and create objects
                while (reader.Read())
                {
                    apiKeySubscription = new APIKeySubscription(id, Convert.ToString(reader["key"]), Convert.ToString(reader["role"]));
                }

                return apiKeySubscription;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public long Add(APIKeySubscription entity)
        {
            //Insert SQL Query
            string insertSQL = "WITH type_id AS (SELECT id FROM exercise.api_key_types WHERE role = @role) INSERT INTO exercise.api_keys(api_key_type_fk, key) VALUES (type_id.id, @key) RETURNING id;";

            //KeyValuePairs List
            LinkedList<KeyValuePair<string, object>> values = new LinkedList<KeyValuePair<string, object>>();
            values.AddFirst(new KeyValuePair<string, object>("role", entity.Role));
            values.AddFirst(new KeyValuePair<string, object>("key", entity.Key));

            return _database.ExecutePreparedStatement(insertSQL, values);
        }

        public bool DeleteAll()
        {
            //Delete ALL SQL Query
            string deleteSQL = "TRUNCATE exercise.api_keys RESTART IDENTITY CASCADE; ALTER SEQUENCE exercise.api_keys_id_seq RESTART WITH 1";
            return _database.ExecuteNonQuery(deleteSQL) > 0;
        }

        public bool Delete(long id)
        {
            //Delete SQL Query
            string deleteSQL = "DELETE FROM exercise.api_keys WHERE id= '" + id + "';";
            return _database.ExecuteNonQuery(deleteSQL) > 0;
        }

        public bool Update(long id, APIKeySubscription entity)
        {
            //Update SQL Query
            string updateSQL = "WITH type_id AS (SELECT id FROM exercise.api_key_types WHERE role = @role) UPDATE exercise.api_keys SET api_key_type_fk= type_id.id, key=@key WHERE id = @id RETURNING *;";

            //KeyValuePairs List
            LinkedList<KeyValuePair<string, object>> values = new LinkedList<KeyValuePair<string, object>>();
            values.AddFirst(new KeyValuePair<string, object>("id", id));
            values.AddFirst(new KeyValuePair<string, object>("role", entity.Role));
            values.AddFirst(new KeyValuePair<string, object>("key", entity.Key));

            return _database.ExecutePreparedStatement(updateSQL, values) != -1;
        }

        public APIKeySubscription? GetByKey(string apiKey)
        {
            //Select SQL Query
            string getSQL = "SELECT exercise.api_keys.id, key, role FROM exercise.api_keys JOIN exercise.api_key_types ON api_key_type_fk = exercise.api_key_types.id WHERE exercise.api_keys.key='" + apiKey + "';";

            //Delegate function to read result rows
            Func<DbDataReader, APIKeySubscription> readerFunction = (reader) =>
            {
                APIKeySubscription apiKeySubscription = null;

                //Loop records and create objects
                while (reader.Read())
                {
                    apiKeySubscription = new APIKeySubscription(Convert.ToInt64(reader["id"]), Convert.ToString(reader["key"]), Convert.ToString(reader["role"]));
                }

                return apiKeySubscription;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }
    }
}
