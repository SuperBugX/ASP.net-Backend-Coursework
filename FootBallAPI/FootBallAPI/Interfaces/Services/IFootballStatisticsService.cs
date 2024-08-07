using FootballAPI.Models.Football;

namespace FootballAPI.Interfaces.Services
{
    public interface IFootballStatisticsService
    {
        public bool DeleteAllStatistics();
        public bool DeleteStatistic(long id);
        public bool UpdateStatistic(long id, FootballMatchStatistics statistic);
        public LinkedList<FootballMatchStatistics> GetAllStatistics();
        public FootballMatchStatistics? GetStatistic(long id);
        public long AddStatistic(FootballMatchStatistics statistics);
    }
}
