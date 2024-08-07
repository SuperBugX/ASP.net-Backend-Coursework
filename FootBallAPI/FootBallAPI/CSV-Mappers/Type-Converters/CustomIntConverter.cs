using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace FootballAPI.TypeConverters
{
    public class CustomIntConverter : DefaultTypeConverter
    {
        //Customer string to int converter method for CSVHelper
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            try
            {
                //Parse int from string
                return (int) Double.Parse(text);
            }
            catch (Exception e)
            {
                //In case of parsing failure, return default value
                return -1;
            }
        }
    }
}
