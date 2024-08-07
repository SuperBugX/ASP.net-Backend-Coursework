namespace FootballAPI.Exceptions.SQL
{
    public class SqlException : Exception
    {
        public SqlException()
        {
        }

        public SqlException(string message) : base(message)
        {
        }
    }
}

