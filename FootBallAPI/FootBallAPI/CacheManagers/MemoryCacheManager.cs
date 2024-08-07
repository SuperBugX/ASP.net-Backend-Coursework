using FootballAPI.Interfaces.CacheManager;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace FootballAPI.Services
{
    public class MemoryCacheManager : ICacheManager
    {
        //Attributes
        private readonly string _name;
        private readonly int _defaultExpirationInSec;
        private readonly int _memLimitInMB;
        private MemoryCache _cache;

        //Methods
        public MemoryCacheManager(string name = "default", int memLimitInMB = 1024, int defaultExpirationInSec = 120)
        {
            _name = name;
            _memLimitInMB = memLimitInMB;
            _defaultExpirationInSec = defaultExpirationInSec;
            _cache = CreateMemoryCache();
        }

        //Method creates and returns a new instance of a MemoryCache object with specific configurations
        private MemoryCache CreateMemoryCache()
        {
            NameValueCollection cacheSettings = new NameValueCollection(2)
            {
                {"cacheMemoryLimitMegabytes", Convert.ToString(_memLimitInMB)},
                {"pollingInterval", "00:00:30"}
            };
            return new MemoryCache(_name + Guid.NewGuid().ToString(), cacheSettings);
        }

        //Method returns the current count of items stored within cache memory
        public long GetCount() => this._cache.GetCount();

        //Method returns an object from memory (as the provided generic type) based on a key value
        //NOTE: null is returned if the key provided does not correspond to an object stored in memory
        public T? Get<T>(string key) where T : class
        {
            var value = this._cache.Get(key);
            if (value != null)
            {
                return value as T;
            }
            return null;
        }

        //Method adds into memory an object with a key value identifier if the key value does not already exist
        public bool Add(string key, object o)
        {
            return this._cache.Add(new CacheItem(key, o), new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(_defaultExpirationInSec)
            });
        }

        //Method adds into memory an object with a key value identifier and a specified experation time in seconds
        //if the key value does not already exist
        public bool Add(string key, object o, int expiration)
        {
            return this._cache.Add(new CacheItem(key, o), new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(expiration)
            });
        }

        //Method inserts or updates in memory an object with a specific key value identifier.
        public void Set(string key, object o)
        {
            Delete(key);
            Add(key, o);
        }

        //Method inserts or updates in memory an object with a specific key value identifier and
        //updates the expiration time in seconds. 
        public void Set(string key, object o, int expiration)
        {
            Delete(key);
            Add(key, o, expiration);
        }

        //Method deletes an object from memory based on the key identifier
        public void Delete(string key)
        {
            this._cache.Remove(key);
        }
    }
}
