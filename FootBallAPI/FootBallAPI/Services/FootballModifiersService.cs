using FootballAPI.API;
using FootballAPI.Interfaces.CacheManager;
using FootballAPI.Interfaces.Services;

namespace FootballAPI.Services
{
    public class FootballModifiersService : IFootballModifiersService
    {
        //Attributes
        private FootballModifiersAPI _footballModifiersAPI;
        private ICacheManager _cacheManager;

        //Methods
        public FootballModifiersService(ICacheManager cacheManager)
        {
            _footballModifiersAPI = new FootballModifiersAPI();
            _cacheManager = cacheManager;
        }

        public async Task<Dictionary<string, string>> GetDivisionModifiersAsync()
        {
            Dictionary<string, string> divisionModifiersMap = null;

            lock (_cacheManager)
            {
                //Check if division modifiers are stored in cache
                divisionModifiersMap = _cacheManager.Get<Dictionary<string, string>>("divisionModifiersMap");

                if (divisionModifiersMap == null)
                {
                    //Get latest modifiers from API
                    divisionModifiersMap = _footballModifiersAPI.GetDivisionModifiersAsync().Result;

                    if (divisionModifiersMap != null)
                    {
                        //Update cache with latest modifiers
                        _cacheManager.Set("divisionModifiersMap", divisionModifiersMap);
                    }
                }
            }

            return divisionModifiersMap;
        }
    }
}
