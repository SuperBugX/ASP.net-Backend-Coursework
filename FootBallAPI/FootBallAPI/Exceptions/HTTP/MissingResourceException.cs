namespace FootballAPI.Exceptions.HTTP
{
    public class MissingResourceException : Exception
    {
        public MissingResourceException()
        {
        }

        public MissingResourceException(string message) : base(message)
        {
        }
    }
}

