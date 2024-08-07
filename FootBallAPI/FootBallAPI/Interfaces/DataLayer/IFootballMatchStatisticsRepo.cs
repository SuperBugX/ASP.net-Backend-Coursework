using FootballAPI.Models.DAO;

namespace FootballAPI.Interfaces.DataLayer
{
    public interface IFootballMatchStatisticsRepo
    {
        public LinkedList<FootballMatchStatisticsDao> All();

        public FootballMatchStatisticsDao? Get(long id);

        public long Add(FootballMatchStatisticsDao entity);

        public bool DeleteAll();

        public bool Delete(long id);

        public bool Update(long id, FootballMatchStatisticsDao entity);
    }
}
