using FootballAPI.Models.DAO;

namespace FootballAPI.Models.Football
{
    public class FootballMatchStatistics
    {
        //Attributes
        /*
            Attendance = Crowd Attendance
            Referee = Match Referee
        */
        public long Id { get; set; }
        public long MatchId { get; set; }
        public int Attendance { get; set; }
        public string Referee { get; set; }

        //Methods
        public FootballMatchStatistics(long id, long matchId, int attendance, string referee)
        {
            Id = id;
            MatchId = matchId;
            Attendance = attendance;
            Referee = referee;
        }

        public FootballMatchStatistics(FootballMatchStatisticsDao dao)
        {
            Id = dao.Id;
            MatchId = dao.MatchId;
            Attendance = dao.Attendance;
            Referee = dao.Referee;
        }

        public FootballMatchStatistics()
        {
            Id = -1;
            MatchId = -1;
            Attendance = 0;
            Referee = "";
        }

        public override bool Equals(object? obj)
        {
            return obj is FootballMatchStatistics statistics &&
                   Id == statistics.Id &&
                   MatchId == statistics.MatchId &&
                   Attendance == statistics.Attendance &&
                   Referee == statistics.Referee;
        }
    }
}
