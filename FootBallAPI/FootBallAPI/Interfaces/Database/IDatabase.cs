using System.Data.Common;

namespace FootballAPI.Interfaces.Database
{
    public interface IDatabase
    {
        public long ExecuteNonQuery(string sql);
        public long ExecutePreparedStatement(string sql, IEnumerable<KeyValuePair<string, object>> values);
        public T? ExecuteDataReader<T>(string sql, Func<DbDataReader, T> reader) where T : class;
    }
}
