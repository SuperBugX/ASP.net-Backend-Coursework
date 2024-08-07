using FootballAPI.Interfaces.Services;
using FootballAPI.Interfaces.Services.ServiceOrchistrators;
using FootballAPI.Models.Football;

namespace FootballAPI.ServiceOrchestrators
{
    public class FootballModifierOrchestrator : IFootballModifiersOrchestrator
    {
        //Attributes
        private IFootballMatchService _footballMatchService;
        private IFootballModifiersService _footballModifiersService;

        //Methods
        public FootballModifierOrchestrator(
            IFootballMatchService footballMatchService,
            IFootballModifiersService footballModifiersService)
        {
            _footballMatchService = footballMatchService;
            _footballModifiersService = footballModifiersService;
        }

        //Method returns football divisions (with teams) that are currently active
        public LinkedList<FootballDivisionTeams> GetActiveDivisionTeams()
        {
            //Gte the latest API division modifiers
            Dictionary<string, string> divisionModifiersMap = _footballModifiersService.GetDivisionModifiersAsync().Result;

            //Determine if any modifiers exist
            if (divisionModifiersMap != null)
            {
                //Create list of only active divisions names
                IEnumerable<String> activeDivisions =
                    divisionModifiersMap.Where(entry => entry.Value.Equals("active")).Select(entry => entry.Key).ToList();

                //Get and return active divisions with teams
                return _footballMatchService.GetActiveDivisionTeams(activeDivisions);
            }
            else
            {
                //Get and return all divisions with teams
                return _footballMatchService.GetAllDivisionTeams();
            }
        }
    }
}
