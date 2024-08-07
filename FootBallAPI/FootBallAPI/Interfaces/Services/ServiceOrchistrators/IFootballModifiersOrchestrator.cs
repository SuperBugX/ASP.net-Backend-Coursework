using FootballAPI.Models.Football;

namespace FootballAPI.Interfaces.Services.ServiceOrchistrators
{
    public interface IFootballModifiersOrchestrator
    {
        public LinkedList<FootballDivisionTeams> GetActiveDivisionTeams();
    }
}
