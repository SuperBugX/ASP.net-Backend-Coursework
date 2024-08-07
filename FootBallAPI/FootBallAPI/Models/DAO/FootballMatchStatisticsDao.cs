using FootballAPI.Models.Football;

namespace FootballAPI.Models.DAO
{
    public class FootballMatchStatisticsDao
    {
        //Attributes
        public long Id { get; set; }
        public long MatchId { get; set; }
        public int Attendance { get; set; }
        public string Referee { get; set; }

        //Methods
        public FootballMatchStatisticsDao(long id, long matchId, int attendance, string referee)
        {
            Id = id;
            MatchId = matchId;
            Attendance = attendance;
            Referee = referee;
        }

        public FootballMatchStatisticsDao(FootballMatchStatistics statistics)
        {
            Id = statistics.Id;
            MatchId = statistics.MatchId;
            Attendance = statistics.Attendance;
            Referee = statistics.Referee;
        }

        public FootballMatchStatisticsDao()
        {
            Id = -1;
            MatchId = -1;
            Attendance = 0;
            Referee = "";
        }

        public override bool Equals(object? obj)
        {
            return obj is FootballMatchStatisticsDao dao &&
                   Id == dao.Id &&
                   MatchId == dao.MatchId &&
                   Attendance == dao.Attendance &&
                   Referee == dao.Referee;
        }
    }
}
