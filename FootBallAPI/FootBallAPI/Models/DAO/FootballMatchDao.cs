using FootballAPI.Models.Football;

namespace FootballAPI.Models.DAO
{
    public class FootballMatchDao
    {
        //Attributes
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string LeagueDivision { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string FtResult { get; set; }
        public string HtResult { get; set; }
        public int FtHomeGoals { get; set; }
        public int FtAwayGoals { get; set; }
        public int HtHomeGoals { get; set; }
        public int HtAwayGoals { get; set; }
        public FootballMatchStatisticsDao Statistics { get; set; }

        //Methods
        public FootballMatchDao(
            long id,
            string leagueDivision,
            DateTime date,
            string homeTeam,
            string awayTeam,
            int ftHomeGoals,
            int ftAwayGoals,
            string fullTimeResult,
            int htHomeGoals,
            int htAwayGoals,
            string halfTimeResult,
            FootballMatchStatisticsDao statistics)
        {
            Id = id;
            LeagueDivision = leagueDivision;
            Date = date;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            FtHomeGoals = ftHomeGoals;
            FtAwayGoals = ftAwayGoals;
            FtResult = fullTimeResult;
            HtHomeGoals = htHomeGoals;
            HtAwayGoals = htAwayGoals;
            HtResult = halfTimeResult;
            Statistics = statistics;
        }

        public FootballMatchDao(FootballMatch match)
        {
            Id = match.Id;
            LeagueDivision = match.LeagueDivision;
            Date = match.Date;
            HomeTeam = match.HomeTeam;
            AwayTeam = match.AwayTeam;
            FtHomeGoals = match.FtHomeGoals;
            FtAwayGoals = match.FtAwayGoals;
            FtResult = match.FtResult;
            HtHomeGoals = match.HtHomeGoals;
            HtAwayGoals = match.HtAwayGoals;
            HtResult = match.HtResult;

            if (match.Statistics != null)
            {
                Statistics = new FootballMatchStatisticsDao(match.Statistics);
            }
            else
            {
                Statistics = null;
            }
        }

        public FootballMatchDao()
        {
            Id = -1;
            LeagueDivision = "";
            Date = new DateTime();
            HomeTeam = "";
            AwayTeam = "";
            FtHomeGoals = 0;
            FtAwayGoals = 0;
            FtResult = "";
            HtHomeGoals = 0;
            HtAwayGoals = 0;
            HtResult = "";
            Statistics = null;
        }

        public override bool Equals(object? obj)
        {
            return obj is FootballMatchDao dao &&
                   Id == dao.Id &&
                   Date == dao.Date &&
                   LeagueDivision == dao.LeagueDivision &&
                   HomeTeam == dao.HomeTeam &&
                   AwayTeam == dao.AwayTeam &&
                   FtResult == dao.FtResult &&
                   HtResult == dao.HtResult &&
                   FtHomeGoals == dao.FtHomeGoals &&
                   FtAwayGoals == dao.FtAwayGoals &&
                   HtHomeGoals == dao.HtHomeGoals &&
                   HtAwayGoals == dao.HtAwayGoals &&
                   EqualityComparer<FootballMatchStatisticsDao>.Default.Equals(Statistics, dao.Statistics);
        }
    }
}
