using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace FootballAPI.TypeConverters
{
    public class CustomDateTimeConverter : DefaultTypeConverter
    {
        //Customer string to DateTime converter method for CSVHelper
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            try
            {
                //Parse string
                return DateTime.Parse(text);
            }
            catch (Exception ex)
            {
                //In case of parsing failure, return default DateTime object
                return new DateTime();
            }
        }
    }
}
