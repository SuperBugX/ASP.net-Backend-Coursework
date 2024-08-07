using FootballAPI.Models.DAO;

namespace FootballAPI.Models.Football
{
    public class FootballMatch
    {
        //Attributes
        /*
            Div = League Division
            Date = Match Date (dd/mm/yy)
            HomeTeam = Home Team
            AwayTeam = Away Team
            FTHG and HG = Full Time Home Team Goals
            FTAG and AG = Full Time Away Team Goals
            FTR and Res = Full Time Result (H=Home Win, D=Draw, A=Away Win)
            HTHG = Half Time Home Team Goals
            HTAG = Half Time Away Team Goals
            HTR = Half Time Result (H=Home Win, D=Draw, A=Away Win)
         */

        //Match Details
        public long Id { get; set; }
        public string LeagueDivision { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        //Full Time Results
        public int FtHomeGoals { get; set; }
        public int FtAwayGoals { get; set; }
        public string FtResult { get; set; }
        //Half Time Results
        public int HtHomeGoals { get; set; }
        public int HtAwayGoals { get; set; }
        public string HtResult { get; set; }
        //Match Statistics
        public FootballMatchStatistics Statistics { get; set; }

        //Methods
        public FootballMatch(
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
            FootballMatchStatistics statistics)
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

        public FootballMatch(FootballMatchDao dao)
        {
            Id = dao.Id;
            LeagueDivision = dao.LeagueDivision;
            Date = dao.Date;
            HomeTeam = dao.HomeTeam;
            AwayTeam = dao.AwayTeam;
            FtHomeGoals = dao.FtHomeGoals;
            FtAwayGoals = dao.FtAwayGoals;
            FtResult = dao.FtResult;
            HtHomeGoals = dao.HtHomeGoals;
            HtAwayGoals = dao.HtAwayGoals;
            HtResult = dao.HtResult;

            if (dao.Statistics != null)
            {
                Statistics = new FootballMatchStatistics(dao.Statistics);
            }
            else
            {
                Statistics = null;
            }
        }

        public FootballMatch()
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
            return obj is FootballMatch match &&
                   Id == match.Id &&
                   LeagueDivision == match.LeagueDivision &&
                   Date == match.Date &&
                   HomeTeam == match.HomeTeam &&
                   AwayTeam == match.AwayTeam &&
                   FtHomeGoals == match.FtHomeGoals &&
                   FtAwayGoals == match.FtAwayGoals &&
                   FtResult == match.FtResult &&
                   HtHomeGoals == match.HtHomeGoals &&
                   HtAwayGoals == match.HtAwayGoals &&
                   HtResult == match.HtResult &&
                   EqualityComparer<FootballMatchStatistics>.Default.Equals(Statistics, match.Statistics);
        }


        public override string ToString()
        {
            return "Id " + Id + " Div:" + LeagueDivision + "Date " + Date.ToString() + "Home Team " + HomeTeam +
                   "Away Team" + AwayTeam + "FtHG" + FtHomeGoals + "FtAG" + FtAwayGoals + "FResult" + FtResult + "HtHomeG" + HtHomeGoals + "HtAwayG" + HtAwayGoals + "HResult" + HtResult + "LAST" + Statistics;
        }
    }
}
