namespace FootballAPI.Interfaces.CacheManager
{
    public interface ICacheManager
    {
        public long GetCount();
        public T? Get<T>(string key) where T : class;
        public bool Add(string key, object o);
        public bool Add(string key, object o, int expiration);
        public void Set(string key, object o);
        public void Set(string key, object o, int expiration);
        public void Delete(string key);
    }
}
