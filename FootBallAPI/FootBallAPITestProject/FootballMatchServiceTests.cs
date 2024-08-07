using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Models.DAO;
using FootballAPI.Models.Football;
using FootballAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FootBallAPITestProject
{
    public class FootBallMatchServiceTests
    {
        private readonly FootballMatchService _matchService;
        private readonly Mock<IFootballMatchRepo> _footballMatchRepo;

        public FootBallMatchServiceTests()
        {
            _footballMatchRepo = new Mock<IFootballMatchRepo>();
            _matchService = new FootballMatchService(_footballMatchRepo.Object);
        }

        [Fact]
        public void Add_Football_Match()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);

            _footballMatchRepo.Setup(x => x.Add(new FootballMatchDao(match))).Returns(match.Id);

            //ACT 
            long newId = _matchService.AddMatch(match);

            //ASSERT
            Assert.Equal(1, newId);
        }

        [Fact]
        public void Delete_Football_Match()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);


            _footballMatchRepo.Setup(x => x.Delete(match.Id)).Returns(true);

            //ACT + ASSERT
            Assert.True(_matchService.DeleteMatch(match.Id));
        }

        [Fact]
        public void Update_Football_Match()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);


            _footballMatchRepo.Setup(x => x.Update(match.Id, new FootballMatchDao(match))).Returns(true);

            //ACT + ASSERT
            Assert.True(_matchService.UpdateMatch(match.Id, match));
        }

        [Fact]
        public void Delete_All_Football_Matches()
        {
            //ARRANGE
            _footballMatchRepo.Setup(x => x.DeleteAll()).Returns(true);

            //ACT + ASSERT
            Assert.True(_matchService.DeleteAllMatches());
        }

        [Fact]
        public void Get_All_Division_Teams()
        {
            //ARRANGE
            LinkedList<FootballDivisionTeams> expectedDivisionTeams = new LinkedList<FootballDivisionTeams>();
            expectedDivisionTeams.AddFirst(new FootballDivisionTeams());

            _footballMatchRepo.Setup(x => x.GetAllDivisionTeams()).Returns(expectedDivisionTeams);

            //ACT 
            LinkedList<FootballDivisionTeams> divisionTeams = _matchService.GetAllDivisionTeams();

            //ASSERT
            Assert.Equal(expectedDivisionTeams, divisionTeams);
        }

        [Fact]
        public void Get_All_Football_Matches()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);

            LinkedList<FootballMatchDao> matchesDao = new LinkedList<FootballMatchDao>();
            matchesDao.AddFirst(new FootballMatchDao(match));

            _footballMatchRepo.Setup(x => x.All()).Returns(matchesDao);

            //ACT 
            LinkedList<FootballMatch> matches = _matchService.GetAllMatches();

            //ASSERT
            LinkedList<FootballMatch> expectedMatches = new LinkedList<FootballMatch>();
            expectedMatches.AddFirst(match);

            Assert.True(matches.First.Value.Equals(match));
        }

        [Fact]
        public void Get_Football_Match()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);

            _footballMatchRepo.Setup(x => x.Get(match.Id))
                .Returns(new FootballMatchDao(match));

            //ACT + ASSERT
            Assert.Equal(match, _matchService.GetMatch(match.Id));
        }


        [Fact]
        public void Get_Football_Match_By_Dates()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);

            LinkedList<FootballMatchDao> matchesDao = new LinkedList<FootballMatchDao>();
            matchesDao.AddFirst(new FootballMatchDao(match));

            _footballMatchRepo.Setup(x => x.GetByDates(date, date.AddDays(62)))
                .Returns(matchesDao);

            //ACT
            LinkedList<FootballMatch> matches = _matchService.GetMatchesByDates(date, date.AddDays(62));

            //ASSERT
            Assert.True(matches.First.Value.Equals(match));
        }

        [Fact]
        public void Get_Football_Match_By_Team_And_Dates()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);

            LinkedList<FootballMatchDao> matchesDao = new LinkedList<FootballMatchDao>();
            matchesDao.AddFirst(new FootballMatchDao(match));

            _footballMatchRepo.Setup(x => x.GetByTeamAndDate("Bury", date, date.AddDays(62)))
                .Returns(matchesDao);

            //ACT
            LinkedList<FootballMatch> matches = _matchService.GetMatchesByTeamAndDate("Bury", date, date.AddDays(62));

            //ASSERT
            Assert.True(matches.First.Value.Equals(match));
        }

        [Fact]
        public void Get_Football_Matches_By_Team()
        {
            //ARRANGE
            DateTime date = DateTime.UtcNow;
            FootballMatch match =
                new FootballMatch(
                    1,
                    "E1",
                    date, "Bury",
                    "Cambridge",
                    1,
                    2,
                    "Win",
                    0,
                    0,
                    "Draw",
                    null);

            LinkedList<FootballMatchDao> matchesDao = new LinkedList<FootballMatchDao>();
            matchesDao.AddFirst(new FootballMatchDao(match));

            _footballMatchRepo.Setup(x => x.GetByTeams("Bury", "Cambridge"))
                .Returns(matchesDao);

            //ACT
            LinkedList<FootballMatch> matches = _matchService.GetMatchesByTeams("Bury", "Cambridge");

            //ASSERT
            Assert.True(matches.First.Value.Equals(match));
        }
    }
}
