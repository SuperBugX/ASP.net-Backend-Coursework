using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Interfaces.Services;
using FootballAPI.Models.Football;
using FootballAPI.Models.DAO;
using FootballAPI.Exceptions.HTTP;

namespace FootballAPI.Services
{
    public class FootballStatisticsService : IFootballStatisticsService
    {
        //Attributes
        private IFootballMatchStatisticsRepo _footballMatchStatisticsRepository;

        //Methods
        public FootballStatisticsService(IFootballMatchStatisticsRepo footballMatchStatisticsRepository)
        {
            _footballMatchStatisticsRepository = footballMatchStatisticsRepository;
        }

        //Method deletes all foot ball matches from storage
        public bool DeleteAllStatistics()
        {
            return _footballMatchStatisticsRepository.DeleteAll();
        }

        //Method returns all football matche statistics in a Enumerable
        public LinkedList<FootballMatchStatistics> GetAllStatistics()
        {
            LinkedList<FootballMatchStatisticsDao> daoMatches = _footballMatchStatisticsRepository.All();
            return new LinkedList<FootballMatchStatistics>(daoMatches.Select(x => new FootballMatchStatistics(x)).ToList());
        }

        //Method returns a single FootballMatchStatistics object based on ID
        public FootballMatchStatistics GetStatistic(long id)
        {
            FootballMatchStatisticsDao statisticsDao = _footballMatchStatisticsRepository.Get(id);

            if (statisticsDao == null)
            {
                throw new MissingResourceException("The resource with ID: " + id + " could not be found");
            }
            else
            {
                return new FootballMatchStatistics(statisticsDao);
            }
        }

        //Method adds a new FootballMatchStatistics object into storage
        public long AddStatistic(FootballMatchStatistics statistic)
        {
            if (statistic != null)
            {
                return _footballMatchStatisticsRepository.Add(new FootballMatchStatisticsDao(statistic));
            }

            throw new ArgumentException("Missing statistic");
        }

        //Method deletes a football match statistic from storage based on ID
        public Boolean DeleteStatistic(long id)
        {
            return _footballMatchStatisticsRepository.Delete(id);
        }

        //Method updates a football match statistic in storage based on ID and a provided FootballMatchStatistics object
        public Boolean UpdateStatistic(long id, FootballMatchStatistics statistic)
        {
            if (statistic != null)
            {
                return _footballMatchStatisticsRepository.Update(id, new FootballMatchStatisticsDao(statistic));
            }

            throw new ArgumentException("Missing statistic");
        }
    }
}
