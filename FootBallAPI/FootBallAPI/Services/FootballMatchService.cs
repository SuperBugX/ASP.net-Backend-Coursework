using FootballAPI.Exceptions.HTTP;
using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Interfaces.Services;
using FootballAPI.Models.DAO;
using FootballAPI.Models.Football;

namespace FootballAPI.Services
{
    public class FootballMatchService : IFootballMatchService
    {
        //Attributes
        private IFootballMatchRepo _footballMatchRepository;

        //Methods
        public FootballMatchService(IFootballMatchRepo footballMatchRepository)
        {
            _footballMatchRepository = footballMatchRepository;
        }

        //Method deletes all Foot ball matches from storage
        //NOTE:All related football match statistics will be deleted
        public bool DeleteAllMatches()
        {
            return _footballMatchRepository.DeleteAll();
        }

        //Method returns all football matches in a Enumerable
        public LinkedList<FootballMatch> GetAllMatches()
        {
            LinkedList<FootballMatchDao> daoMatches = _footballMatchRepository.All();
            return new LinkedList<FootballMatch>(daoMatches.Select(x => new FootballMatch(x)).ToList());
        }

        //Method returns a single FootballMatch object based on ID
        public FootballMatch GetMatch(long id)
        {
            FootballMatchDao matchDao = _footballMatchRepository.Get(id);

            if (matchDao == null)
            {
                throw new MissingResourceException("The resource with ID: " + id + " could not be found");
            }
            else
            {
                return new FootballMatch(matchDao);
            }
        }

        //Method adds a new FootballMatch object into storage
        public long AddMatch(FootballMatch match)
        {
            if (match != null)
            {
                return _footballMatchRepository.Add(new FootballMatchDao(match));
            }

            throw new ArgumentException("Missing match");
        }

        //Method deletes a football match from storage based on ID
        public Boolean DeleteMatch(long id)
        {
            return _footballMatchRepository.Delete(id);
        }

        //Method updates a football match in storage based on ID and a provided FootballMatch object
        public Boolean UpdateMatch(long id, FootballMatch match)
        {
            if (match != null)
            {
                return _footballMatchRepository.Update(id, new FootballMatchDao(match));
            }
            throw new ArgumentException("Missing match");
        }

        /*
            Method returns a number of FootballMatch objects found within a start and end date.

            NOTE: Atleast one DateTime object must be provided, the missing date argument is replaced 
            with a maximum or minimum date respectively.
        */
        public LinkedList<FootballMatch> GetMatchesByDates(DateTime? startDate, DateTime? endDate)
        {
            //Input Date Validation
            if (!startDate.HasValue && !endDate.HasValue)
            {
                throw new ArgumentException("Must provide at least one start or end date");
            }
            else
            {
                if (!startDate.HasValue)
                {
                    startDate = DateTime.MinValue;
                }

                if (!endDate.HasValue)
                {
                    endDate = DateTime.MaxValue;
                }
            }

            LinkedList<FootballMatchDao> daoMatches = _footballMatchRepository.GetByDates((DateTime)startDate, (DateTime)endDate);
            return new LinkedList<FootballMatch>(daoMatches.Select(x => new FootballMatch(x)).ToList());
        }

        /*
            Method returns a number of FootballMatch objects based on the 
            specified Home Team and Away Team names.

            NOTE: Atleast one team name must be provided, the missing team name is ignored.
        */
        public LinkedList<FootballMatch> GetMatchesByTeams(string homeTeam, string awayTeam)
        {
            //Input Team Validation
            if (String.IsNullOrEmpty(homeTeam) && String.IsNullOrEmpty(awayTeam))
            {
                throw new ArgumentException("Must provide atleast one team");
            }

            LinkedList<FootballMatchDao> daoMatches = _footballMatchRepository.GetByTeams(homeTeam, awayTeam);
            return new LinkedList<FootballMatch>(daoMatches.Select(x => new FootballMatch(x)).ToList());
        }

        /*
            Method returns a number of FootballMatch objects based on the team name 
            provided and start and end date range.

            NOTE: The team name is both used for Home and Away football matches.
            Atleast one DateTime object must be provided, the missing date argument is replaced 
            with a maximum or minimum date respectively.
        */
        public LinkedList<FootballMatch> GetMatchesByTeamAndDate(string team, DateTime? startDate, DateTime? endDate)
        {
            //Input Date Validation
            if (!startDate.HasValue && !endDate.HasValue)
            {
                throw new ArgumentException("Must provide at least one start or end date");
            }
            else
            {
                if (!startDate.HasValue)
                {
                    startDate = DateTime.MinValue;
                }

                if (!endDate.HasValue)
                {
                    endDate = DateTime.MaxValue;
                }
            }

            if (String.IsNullOrEmpty(team))
            {
                throw new ArgumentException("Must provide a team name");
            }

            LinkedList<FootballMatchDao> daoMatches = _footballMatchRepository.GetByTeamAndDate(team, (DateTime)startDate, (DateTime)endDate);
            return new LinkedList<FootballMatch>(daoMatches.Select(x => new FootballMatch(x)).ToList());
        }

        /*
            Method returns a number of FootballDivisionTeams objects each representing
            a league division and their participating teams.
        */
        public LinkedList<FootballDivisionTeams> GetAllDivisionTeams()
        {
            return _footballMatchRepository.GetAllDivisionTeams();
        }

        /*
            Method returns a number of FootballDivisionTeams objects each representing
            a league division and their participating teams.
        */
        public LinkedList<FootballDivisionTeams> GetActiveDivisionTeams(IEnumerable<String> activeDivisions)
        {
            return _footballMatchRepository.GetActiveDivisionTeams(activeDivisions);
        }
    }
}
