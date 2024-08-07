using System.Diagnostics.CodeAnalysis;
using FootballAPI.TypeConverters;
using CsvHelper.Configuration;
using FootballAPI.Models.Football;

namespace FootballAPI.CSVMappers
{
    [ExcludeFromCodeCoverage]
    public class FootballMatchStatisticsMap : ClassMap<FootballMatchStatistics>
    {
        /*
            Method is used to map CSV fields for the creation of new FootballMatchStatistics objects with CSVHelper.
            In the event of missing or incorrect CSV data, default values for int is "-1" and string "".
            In the event of bad referee data, the string value "N/A" is used.
        */
        public FootballMatchStatisticsMap()
        {
            Map(m => m.Id).Ignore().Default(-1);
            Map(m => m.MatchId).Ignore().Default(-1);
            Map(m => m.Attendance).Name("Attendance").Optional().Default(-1).TypeConverter<CustomIntConverter>();
            Map(m => m.Referee).Name("Referee").Optional().Default("N/A");
        }
    }
}
