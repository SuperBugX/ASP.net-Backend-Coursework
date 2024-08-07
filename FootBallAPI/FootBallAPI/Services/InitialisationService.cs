using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using FootballAPI.Interfaces.Services;
using FootballAPI.Models.Football;
using FootballAPI.CSVMappers;

namespace FootballAPI.Services
{
    public class InitialisationService : IHostedService
    {
        //Attributes
        private IFootballMatchService _footballMatchService;
        private IFootballStatisticsService _footballStatisticsService;

        //Methods
        public InitialisationService(IFootballMatchService footballMatchService, IFootballStatisticsService footballStatisticsService)
        {
            _footballMatchService = footballMatchService;
            _footballStatisticsService = footballStatisticsService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task csvParsingTask = Task.Run(() =>
            {
                //Reset Database
                _footballMatchService.DeleteAllMatches();
                _footballStatisticsService.DeleteAllStatistics();

                //CSV parsing configuration code
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    TrimOptions = TrimOptions.Trim,
                    IncludePrivateMembers = true,
                    ShouldSkipRecord = record => record.Record.GetValue(0).Equals("")
                };

                //Get current app directory
                string startupPath = System.IO.Directory.GetCurrentDirectory();
                //Set "Data" folder path for CSV within app
                string csvDataPath = "\\src\\Data";

                //Scan for directories within the provided path
                string[] dirs = Directory.GetDirectories(startupPath + csvDataPath, "*", System.IO.SearchOption.TopDirectoryOnly);

                //Loop through directories
                foreach (string dir in dirs)
                {
                    //Scan for CSV files within each directory
                    string[] files = Directory.GetFiles(dir, "*.csv");

                    //Loop through CSV files
                    foreach (string file in files)
                    {
                        //Parsing and Insertion
                        try
                        {
                            using (var reader = new StreamReader(file))
                            using (var csvReader = new CsvReader(reader, csvConfig))
                            {
                                //Apply CSV mapping classes
                                csvReader.Context.RegisterClassMap<FootballMatchMap>();
                                csvReader.Context.RegisterClassMap<FootballMatchStatisticsMap>();

                                //Parse current CSV into list
                                IEnumerable<FootballMatch> csvRecordList = csvReader.GetRecords<FootballMatch>().ToList();

                                //DB Insertion
                                Task task = Task.Run(() =>
                                {
                                    foreach (FootballMatch match in csvRecordList)
                                    {
                                        //Check if nested statistics object is default in values
                                        if (match.Statistics.Attendance == -1 && match.Statistics.Referee != null && match.Statistics.Referee.Equals("N/A"))
                                        {
                                            match.Statistics = null;
                                        }

                                        _footballMatchService.AddMatch(match);
                                    }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            //Error
                            Console.WriteLine("File parsing error: " + ex.Message);
                        }
                    }
                }
            });

            return csvParsingTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            return;
        }
    }
}
