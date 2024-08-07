namespace FootballAPI.Interfaces.Services
{
    public interface IAuthorisationService
    {
        public string GetRoleByAPIKey(string apiKey);
        public bool IsDeveloper(string apiKey);
        public bool IsCustomer(string apiKey);
    }
}
