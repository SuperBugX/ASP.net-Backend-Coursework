using FootballAPI.Models.DAO;
using FootballAPI.Models.Football;

namespace FootballAPI.Interfaces.DataLayer
{
    public interface IFootballMatchRepo
    {
        public LinkedList<FootballMatchDao> All();

        public FootballMatchDao? Get(long id);

        public long Add(FootballMatchDao entity);

        public bool DeleteAll();

        public bool Delete(long id);

        public bool Update(long id, FootballMatchDao entity);

        public LinkedList<FootballMatchDao> GetByDates(DateTime startDate, DateTime endDate);

        public LinkedList<FootballMatchDao> GetByTeams(string homeTeam, string awayTeam);

        public LinkedList<FootballMatchDao> GetByTeamAndDate(string team, DateTime startDate, DateTime endDate);

        public LinkedList<FootballDivisionTeams> GetAllDivisionTeams();
        public LinkedList<FootballDivisionTeams> GetActiveDivisionTeams(IEnumerable<String> activeDivisions);
    }
}
