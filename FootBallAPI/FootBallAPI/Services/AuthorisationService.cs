using FootballAPI.Interfaces.Database;
using FootballAPI.Interfaces.Services;
using FootballAPI.Models.API;
using FootballAPI.Repositories.API;

namespace FootballAPI.Services
{
    public class AuthorisationService : IAuthorisationService
    {
        //Attributes
        private APIKeyRepository _apiKeyRepository;

        //Methods
        public AuthorisationService(IDatabase database)
        {
            _apiKeyRepository = new APIKeyRepository(database);
        }

        //Method returns a string indicating a role based on a provided string apiKey.
        //An empty string is returned if the provided apiKey is invalid.
        public string GetRoleByAPIKey(string apiKey)
        {
            //Get APIKeySubscription object based on apiKey
            APIKeySubscription apiKeySubscription = _apiKeyRepository.GetByKey(apiKey);

            //Return role;
            if (apiKeySubscription != null)
            {
                return apiKeySubscription.Role;
            }

            return "";
        }

        //Method returns a boolean value for if the role of a user is a "developer"
        //based on a provided string apiKey.
        public Boolean IsDeveloper(string apiKey)
        {
            //Get APIKeySubscription object based on apiKey
            APIKeySubscription apiKeySubscription = _apiKeyRepository.GetByKey(apiKey);

            //Return boolean value
            if (apiKeySubscription != null)
            {
                return apiKeySubscription.Role.Equals("developer");
            }
            return false;
        }

        //Method returns a boolean value for if the role of a user is a "customer"
        //based on a provided string apiKey.
        public Boolean IsCustomer(string apiKey)
        {
            //Get APIKeySubscription object based on apiKey
            APIKeySubscription apiKeySubscription = _apiKeyRepository.GetByKey(apiKey);

            //Return boolean value
            if (apiKeySubscription != null)
            {
                return apiKeySubscription.Role.Equals("customer");
            }
            return false;
        }
    }
}
