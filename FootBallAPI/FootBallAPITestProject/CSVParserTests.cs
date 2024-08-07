using FootballAPI.TypeConverters;
using System;
using Xunit;

namespace FootBallAPITestProject
{
    public class CSVParserTests
    {
        [Theory]
        [InlineData("2020-03-05", 2020, 03, 05)] // yyyy-MM-dd
        public void CSVHelper_DateTime_Converter_Parses_String(string fromFile, int year, int month, int day)
        {
            //ARRANGE
            CustomDateTimeConverter converter = new CustomDateTimeConverter();
            DateTime validDate = new DateTime(year, month, day);

            //ACT
            DateTime parsedDate = (DateTime)converter.ConvertFromString(fromFile, null, null);

            //ASSERT
            Assert.Equal(validDate, parsedDate);
        }

        [Fact]
        public void CSVHelper_Int_Converter_Converts_Parses_String()
        {
            //ARRANGE
            CustomIntConverter converter = new CustomIntConverter();
            string stringNumber = "324524.323";

            //ACT
            int value = (int)converter.ConvertFromString(stringNumber, null, null);

            //ASSERT
            Assert.Equal(324524, value);
        }
    }
}
