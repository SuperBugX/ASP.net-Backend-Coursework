using System.Diagnostics.CodeAnalysis;
using FootballAPI.TypeConverters;
using CsvHelper.Configuration;
using FootballAPI.Models.Football;

namespace FootballAPI.CSVMappers
{
    [ExcludeFromCodeCoverage]
    public class FootballMatchMap : ClassMap<FootballMatch>
    {
        /*
            Method is used to map CSV fields for the creation of new FootballMatch objects with CSVHelper.
            In the event of missing or incorrect CSV data, default values for int is "-1" and string "".
        */
        public FootballMatchMap()
        {
            Map(m => m.Id).Ignore();
            Map(m => m.LeagueDivision).Name("Div");
            Map(m => m.Date).Name("Date").TypeConverter<CustomDateTimeConverter>();
            Map(m => m.HomeTeam).Name("HomeTeam");
            Map(m => m.AwayTeam).Name("AwayTeam");
            Map(m => m.FtHomeGoals).Name("FTHG").Optional().Default(-1);
            Map(m => m.FtAwayGoals).Name("FTAG").Optional().Default(-1);
            Map(m => m.FtResult).Name("FTR").Optional().Default("");
            Map(m => m.HtHomeGoals).Name("HTHG").Optional().Default(-1);
            Map(m => m.HtAwayGoals).Name("HTAG").Optional().Default(-1);
            Map(m => m.HtResult).Name("HTR").Optional().Default("");
            References<FootballMatchStatisticsMap>(m => m.Statistics);
        }
    }
}
