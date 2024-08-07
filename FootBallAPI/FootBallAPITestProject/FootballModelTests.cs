using FootballAPI.Models.DAO;
using FootballAPI.Models.Football;
using System;
using Xunit;

namespace FootBallAPITestProject
{
    public class FootballModelTests
    {
        [Fact]
        public void Football_Object_Converts_To_Dao()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match = new FootballMatch(
                1,
                "E1",
                date,
                "Bury",
                "Cambridge",
                1,
                2,
                "Win",
                0,
                0,
                "Draw",
                null);

            FootballMatchDao expectedDao = new FootballMatchDao(
                1,
                "E1",
                date,
                "Bury",
                "Cambridge",
                1,
                2,
                "Win",
                0,
                0,
                "Draw",
                null);

            //ACT
            FootballMatchDao dao = new FootballMatchDao(match);

            //ASSERT
            Assert.Equal(expectedDao, dao);
        }

        [Fact]
        public void Football_Dao_Converts_To_Object()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatchDao dao = new FootballMatchDao(
                1,
                "E1",
                date,
                "Bury",
                "Cambridge",
                1,
                2,
                "Win",
                0,
                0,
                "Draw",
                null);

            FootballMatch expectedMatch = new FootballMatch(
                1,
                "E1",
                date,
                "Bury",
                "Cambridge",
                1,
                2,
                "Win",
                0,
                0,
                "Draw",
                null);

            //ACT
            FootballMatch match = new FootballMatch(dao);

            //ASSERT
            Assert.Equal(expectedMatch, match);
        }

        [Fact]
        public void Football_Statistics_Object_Converts_To_Dao()
        {
            //ARRANGE
            FootballMatchStatistics stats = new FootballMatchStatistics(1, 2, 435653, "John");
            FootballMatchStatisticsDao expectedDao = new FootballMatchStatisticsDao(1, 2, 435653, "John");

            //ACT
            FootballMatchStatisticsDao dao = new FootballMatchStatisticsDao(stats);

            //ASSERT
            Assert.Equal(expectedDao, dao);
        }

        [Fact]
        public void Football_Statistics_Dao_Converts_To_Object()
        {
            //ARRANGE
            FootballMatchStatisticsDao dao = new FootballMatchStatisticsDao(1, 2, 435653, "John");
            FootballMatchStatistics expectedStats = new FootballMatchStatistics(1, 2, 435653, "John");

            //ACT
            FootballMatchStatistics stats = new FootballMatchStatistics(dao);

            //ASSERT
            Assert.Equal(expectedStats, stats);
        }
    }
}