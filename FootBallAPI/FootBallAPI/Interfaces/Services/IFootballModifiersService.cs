namespace FootballAPI.Interfaces.Services
{
    public interface IFootballModifiersService
    {
        public Task<Dictionary<string, string>> GetDivisionModifiersAsync();
    }
}
