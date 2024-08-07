using FootballAPI.Models.Football;

namespace FootballAPI.Interfaces.Services
{
    public interface IFootballMatchService
    {
        public bool DeleteAllMatches();
        public bool DeleteMatch(long id);
        public bool UpdateMatch(long id, FootballMatch match);
        public long AddMatch(FootballMatch match);
        public FootballMatch? GetMatch(long id);
        public LinkedList<FootballMatch> GetAllMatches();
        public LinkedList<FootballMatch> GetMatchesByDates(DateTime? startDate, DateTime? endDate);
        public LinkedList<FootballMatch> GetMatchesByTeams(string? homeTeam, string? awayTeam);
        public LinkedList<FootballMatch> GetMatchesByTeamAndDate(string team, DateTime? startDate, DateTime? endDate);
        public LinkedList<FootballDivisionTeams> GetAllDivisionTeams();
        public LinkedList<FootballDivisionTeams> GetActiveDivisionTeams(IEnumerable<string> activeDivisions);
    }
}
