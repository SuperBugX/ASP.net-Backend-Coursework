using System.Collections.Generic;
using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Models.DAO;
using FootballAPI.Models.Football;
using FootballAPI.Services;
using Moq;
using Xunit;

namespace FootBallAPITestProject
{
    public class FootBallStatisticsServiceTests
    {
        private readonly FootballStatisticsService _statisticsService;
        private readonly Mock<IFootballMatchStatisticsRepo> _footballMatchStatisticsRepo;

        public FootBallStatisticsServiceTests()
        {
            _footballMatchStatisticsRepo = new Mock<IFootballMatchStatisticsRepo>();
            _statisticsService = new FootballStatisticsService(_footballMatchStatisticsRepo.Object);
        }

        [Fact]
        public void Delete_All_Statistics()
        {
            //ARRANGE
            _footballMatchStatisticsRepo.Setup(x => x.DeleteAll()).Returns(true);

            //ACT + ASSERT
            Assert.True(_statisticsService.DeleteAllStatistics());
        }

        [Fact]
        public void Delete_Statistic()
        {
            //ARRANGE
            FootballMatchStatistics stats = new FootballMatchStatistics(1, 2, 435653, "John");

            _footballMatchStatisticsRepo.Setup(x => x.Delete(stats.Id)).Returns(true);

            //ACT + ASSERT
            Assert.True(_statisticsService.DeleteStatistic(stats.Id));
        }

        [Fact]
        public void Update_Statistics()
        {
            //ARRANGE
            FootballMatchStatistics stats = new FootballMatchStatistics(1, 2, 435653, "John");

            _footballMatchStatisticsRepo.Setup(x => x.Update(stats.Id, new FootballMatchStatisticsDao(stats))).Returns(true);

            //ACT + ASSERT
            Assert.True(_statisticsService.UpdateStatistic(stats.Id, stats));
        }

        [Fact]
        public void Get_All_Statistics()
        {
            //ARRANGE
            FootballMatchStatistics stats = new FootballMatchStatistics(1, 2, 435653, "John");

            LinkedList<FootballMatchStatisticsDao> statsDaoList = new LinkedList<FootballMatchStatisticsDao>();
            statsDaoList.AddFirst(new FootballMatchStatisticsDao(stats));

            _footballMatchStatisticsRepo.Setup(x => x.All()).Returns(statsDaoList);

            //ACT 
            LinkedList<FootballMatchStatistics> statsList = _statisticsService.GetAllStatistics();

            //ASSERT
            Assert.True(statsList.First.Value.Equals(stats));
        }

        [Fact]
        public void Get_Statistic()
        {
            //ARRANGE
            FootballMatchStatistics stats = new FootballMatchStatistics(1, 2, 435653, "John");

            _footballMatchStatisticsRepo.Setup(x => x.Get(stats.Id)).Returns(new FootballMatchStatisticsDao(stats));

            //ACT + ASSERT
            Assert.Equal(stats, _statisticsService.GetStatistic(stats.Id));
        }

        [Fact]
        public void Add_Statistic()
        {
            //ARRANGE
            FootballMatchStatistics stats = new FootballMatchStatistics(1, 2, 435653, "John");

            _footballMatchStatisticsRepo.Setup(x => x.Add(new FootballMatchStatisticsDao(stats))).Returns(stats.Id);

            //ACT + ASSERT
            Assert.Equal(stats.Id, _statisticsService.AddStatistic(stats));
        }
    }
}
