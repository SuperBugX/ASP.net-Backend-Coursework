using FootballAPI.Interfaces.Database;
using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Models.DAO;
using FootballAPI.Models.Football;
using System.Data.Common;

namespace FootballAPI.Repositories.Football
{
    public class FootballMatchRepository : IRepository<FootballMatchDao>, IFootballMatchRepo
    {
        //Attributes
        private static IDatabase _database;

        //Methods
        public FootballMatchRepository(IDatabase database)
        {
            _database = database;
        }

        public LinkedList<FootballMatchDao> All()
        {
            //Select SQL Query
            string getAllSQL = "SELECT * FROM exercise.football_matches";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballMatchDao>> readerFunction = (reader) =>
            {
                LinkedList<FootballMatchDao> footballMatches = new LinkedList<FootballMatchDao>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballMatchDao match = new FootballMatchDao();
                    match.Id = Convert.ToInt64(reader["id"]);
                    match.LeagueDivision = Convert.ToString(reader["division"]);
                    match.Date = Convert.ToDateTime(reader["date"]);
                    match.HomeTeam = Convert.ToString(reader["home_team"]);
                    match.AwayTeam = Convert.ToString(reader["away_team"]);
                    match.FtAwayGoals = Convert.ToInt32(reader["ft_away_goals"]);
                    match.FtHomeGoals = Convert.ToInt32(reader["ft_home_goals"]);
                    match.FtResult = Convert.ToString(reader["ft_result"]);
                    match.HtAwayGoals = Convert.ToInt32(reader["ht_away_goals"]);
                    match.HtHomeGoals = Convert.ToInt32(reader["ht_home_goals"]);
                    match.HtResult = Convert.ToString(reader["ht_result"]);
                    match.Statistics = null;
                    footballMatches.AddLast(match);
                }

                return footballMatches;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getAllSQL, readerFunction);
        }

        public FootballMatchDao? Get(long id)
        {
            //Select SQL Query
            string getSQL = "SELECT * FROM exercise.football_matches WHERE id=" + id + ";";

            //Delegate function to read result rows
            Func<DbDataReader, FootballMatchDao?> readerFunction = (reader) =>
            {
                FootballMatchDao match = null;

                //Loop records and create objects
                while (reader.Read())
                {
                    match = new FootballMatchDao();
                    match.Id = id;
                    match.LeagueDivision = Convert.ToString(reader["division"]);
                    match.Date = Convert.ToDateTime(reader["date"]);
                    match.HomeTeam = Convert.ToString(reader["home_team"]);
                    match.AwayTeam = Convert.ToString(reader["away_team"]);
                    match.FtAwayGoals = Convert.ToInt32(reader["ft_away_goals"]);
                    match.FtHomeGoals = Convert.ToInt32(reader["ft_home_goals"]);
                    match.FtResult = Convert.ToString(reader["ft_result"]);
                    match.HtAwayGoals = Convert.ToInt32(reader["ht_away_goals"]);
                    match.HtHomeGoals = Convert.ToInt32(reader["ht_home_goals"]);
                    match.HtResult = Convert.ToString(reader["ht_result"]);
                    match.Statistics = null;
                }

                return match;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public long Add(FootballMatchDao entity)
        {
            //Insert SQL Query
            string insertSQL;

            //KeyValuePairs List
            LinkedList<KeyValuePair<string, object>> values = new LinkedList<KeyValuePair<string, object>>();
            values.AddFirst(new KeyValuePair<string, object>("division", entity.LeagueDivision));
            values.AddFirst(new KeyValuePair<string, object>("date", entity.Date));
            values.AddFirst(new KeyValuePair<string, object>("home_team", entity.HomeTeam));
            values.AddFirst(new KeyValuePair<string, object>("away_team", entity.AwayTeam));
            values.AddFirst(new KeyValuePair<string, object>("ft_home_goals", entity.FtHomeGoals));
            values.AddFirst(new KeyValuePair<string, object>("ft_away_goals", entity.FtAwayGoals));
            values.AddFirst(new KeyValuePair<string, object>("ht_home_goals", entity.HtHomeGoals));
            values.AddFirst(new KeyValuePair<string, object>("ht_away_goals", entity.HtAwayGoals));
            values.AddFirst(new KeyValuePair<string, object>("ft_result", entity.FtResult));
            values.AddFirst(new KeyValuePair<string, object>("ht_result", entity.HtResult));

            //Adapt Insert SQL Query based on if the FootballMatchDao object contains a valid FootballMatchStatisticsDao object
            if (entity.Statistics == null)
            {
                insertSQL = "INSERT INTO exercise.football_matches(division, date, home_team, away_team, ft_home_goals, ft_away_goals, ht_home_goals, ht_away_goals, ft_result, ht_result) VALUES(@division, @date, @home_team, @away_team, @ft_home_goals, @ft_away_goals, @ht_home_goals, @ht_away_goals, @ft_result, @ht_result) RETURNING id;";
            }
            else
            {
                insertSQL = "BEGIN; INSERT INTO exercise.football_matches(division, date, home_team, away_team, ft_home_goals, ft_away_goals, ht_home_goals, ht_away_goals, ft_result, ht_result) VALUES(@division, @date, @home_team, @away_team, @ft_home_goals, @ft_away_goals, @ht_home_goals, @ht_away_goals, @ft_result, @ht_result) RETURNING id; INSERT INTO exercise.football_stats(football_match_fk, attendance, referee) VALUES(currval('exercise.football_matches_id_seq'), @attendance, @referee); COMMIT;";
                values.AddFirst(new KeyValuePair<string, object>("attendance", entity.Statistics.Attendance));
                values.AddFirst(new KeyValuePair<string, object>("referee", entity.Statistics.Referee));
            }

            //Execute SQL Query and get result
            return _database.ExecutePreparedStatement(insertSQL, values);
        }

        public bool DeleteAll()
        {
            //Delete ALL SQL Query
            string deleteSQL = "TRUNCATE exercise.football_matches RESTART IDENTITY CASCADE; ALTER SEQUENCE exercise.football_matches_id_seq RESTART WITH 1;";
            return _database.ExecuteNonQuery(deleteSQL) > 0;
        }

        public bool Delete(long id)
        {
            //Delete SQL Query
            string deleteSQL = "DELETE FROM exercise.football_matches WHERE id= " + id + ";";
            return _database.ExecuteNonQuery(deleteSQL) > 0;
        }

        public bool Update(long id, FootballMatchDao entity)
        {
            //Update SQL Query
            string updateSQL = "UPDATE exercise.football_matches SET division= @division, date= @date, home_team= @home_team, away_team= @away_team, ft_home_goals= @ft_home_goals, ft_away_goals= @ft_away_goals, ht_home_goals= @ht_home_goals, ht_away_goals= @ht_away_goals, ft_result= @ft_result, ht_result= @ht_result WHERE id = @id RETURNING *;";

            //KeyValuePairs List
            LinkedList<KeyValuePair<string, object>> values = new LinkedList<KeyValuePair<string, object>>();
            values.AddFirst(new KeyValuePair<string, object>("id", id));
            values.AddFirst(new KeyValuePair<string, object>("division", entity.LeagueDivision));
            values.AddFirst(new KeyValuePair<string, object>("date", entity.Date));
            values.AddFirst(new KeyValuePair<string, object>("home_team", entity.HomeTeam));
            values.AddFirst(new KeyValuePair<string, object>("away_team", entity.AwayTeam));
            values.AddFirst(new KeyValuePair<string, object>("ft_home_goals", entity.FtHomeGoals));
            values.AddFirst(new KeyValuePair<string, object>("ft_away_goals", entity.FtAwayGoals));
            values.AddFirst(new KeyValuePair<string, object>("ht_home_goals", entity.HtHomeGoals));
            values.AddFirst(new KeyValuePair<string, object>("ht_away_goals", entity.HtAwayGoals));
            values.AddFirst(new KeyValuePair<string, object>("ft_result", entity.FtResult));
            values.AddFirst(new KeyValuePair<string, object>("ht_result", entity.HtResult));

            //Execute SQL Query and get result
            return _database.ExecutePreparedStatement(updateSQL, values) != -1;
        }

        public LinkedList<FootballMatchDao> GetByDates(DateTime startDate, DateTime endDate)
        {
            //Select SQL Query
            string getSQL = "SELECT * FROM exercise.football_matches WHERE date BETWEEN '" + startDate.ToString("yyyy/MM/dd") + "' AND '" + endDate.ToString("yyyy/MM/dd") + "';";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballMatchDao>> readerFunction = (reader) =>
            {
                LinkedList<FootballMatchDao> footballMatches = new LinkedList<FootballMatchDao>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballMatchDao match = new FootballMatchDao();
                    match.Id = Convert.ToInt64(reader["id"]);
                    match.LeagueDivision = Convert.ToString(reader["division"]);
                    match.Date = Convert.ToDateTime(reader["date"]);
                    match.HomeTeam = Convert.ToString(reader["home_team"]);
                    match.AwayTeam = Convert.ToString(reader["away_team"]);
                    match.FtAwayGoals = Convert.ToInt32(reader["ft_away_goals"]);
                    match.FtHomeGoals = Convert.ToInt32(reader["ft_home_goals"]);
                    match.FtResult = Convert.ToString(reader["ft_result"]);
                    match.HtAwayGoals = Convert.ToInt32(reader["ht_away_goals"]);
                    match.HtHomeGoals = Convert.ToInt32(reader["ht_home_goals"]);
                    match.HtResult = Convert.ToString(reader["ht_result"]);
                    match.Statistics = null;
                    footballMatches.AddLast(match);
                }

                return footballMatches;
            };


            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public LinkedList<FootballMatchDao> GetByTeams(string homeTeam, string awayTeam)
        {
            //Select SQL Query
            string getSQL;
            bool homeTeamExists = !homeTeam.Equals("");
            bool awayTeamExists = !awayTeam.Equals("");

            //Adapt Select SQL Query based on the provided team names
            if (homeTeamExists && awayTeamExists)
            {
                getSQL = "SELECT * FROM exercise.football_matches WHERE home_team= '" + homeTeam + "' AND away_team= '" + awayTeam + "';";
            }
            else if (homeTeamExists && !awayTeamExists)
            {
                getSQL = "SELECT * FROM exercise.football_matches WHERE home_team= '" + homeTeam + "';";
            }
            else
            {
                getSQL = "SELECT * FROM exercise.football_matches WHERE away_team= '" + awayTeam + "';";
            }

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballMatchDao>> readerFunction = (reader) =>
            {
                LinkedList<FootballMatchDao> footballMatches = new LinkedList<FootballMatchDao>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballMatchDao match = new FootballMatchDao();
                    match.Id = Convert.ToInt64(reader["id"]);
                    match.LeagueDivision = Convert.ToString(reader["division"]);
                    match.Date = Convert.ToDateTime(reader["date"]);
                    match.HomeTeam = Convert.ToString(reader["home_team"]);
                    match.AwayTeam = Convert.ToString(reader["away_team"]);
                    match.FtAwayGoals = Convert.ToInt32(reader["ft_away_goals"]);
                    match.FtHomeGoals = Convert.ToInt32(reader["ft_home_goals"]);
                    match.FtResult = Convert.ToString(reader["ft_result"]);
                    match.HtAwayGoals = Convert.ToInt32(reader["ht_away_goals"]);
                    match.HtHomeGoals = Convert.ToInt32(reader["ht_home_goals"]);
                    match.HtResult = Convert.ToString(reader["ht_result"]);
                    match.Statistics = null;
                    footballMatches.AddLast(match);
                }

                return footballMatches;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public LinkedList<FootballMatchDao> GetByTeamAndDate(string team, DateTime startDate, DateTime endDate)
        {
            //Select SQL Query
            string getSQL = "SELECT * FROM exercise.football_matches WHERE date BETWEEN '" + startDate.ToString("yyyy/MM/dd") + "' AND '" + endDate.ToString("yyyy/MM/dd") + "' AND (home_team = '" + team + "' OR away_team= '" + team + "');";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballMatchDao>> readerFunction = (reader) =>
            {
                LinkedList<FootballMatchDao> footballMatches = new LinkedList<FootballMatchDao>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballMatchDao match = new FootballMatchDao();
                    match.Id = Convert.ToInt64(reader["id"]);
                    match.LeagueDivision = Convert.ToString(reader["division"]);
                    match.Date = Convert.ToDateTime(reader["date"]);
                    match.HomeTeam = Convert.ToString(reader["home_team"]);
                    match.AwayTeam = Convert.ToString(reader["away_team"]);
                    match.FtAwayGoals = Convert.ToInt32(reader["ft_away_goals"]);
                    match.FtHomeGoals = Convert.ToInt32(reader["ft_home_goals"]);
                    match.FtResult = Convert.ToString(reader["ft_result"]);
                    match.HtAwayGoals = Convert.ToInt32(reader["ht_away_goals"]);
                    match.HtHomeGoals = Convert.ToInt32(reader["ht_home_goals"]);
                    match.HtResult = Convert.ToString(reader["ht_result"]);
                    match.Statistics = null;
                    footballMatches.AddLast(match);
                }

                return footballMatches;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public LinkedList<FootballDivisionTeams> GetAllDivisionTeams()
        {
            //Select SQL Query
            string getSQL = "SELECT division, string_agg(home_team, ',') as home_teams, string_agg(away_team, ',') as away_teams FROM exercise.football_matches GROUP BY division;";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballDivisionTeams>> readerFunction = (reader) =>
            {
                LinkedList<FootballDivisionTeams> divisionTeamsList = new LinkedList<FootballDivisionTeams>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballDivisionTeams divisionTeams = new FootballDivisionTeams();

                    divisionTeams.Division = Convert.ToString(reader["division"]);
                    string homeTeams = Convert.ToString(reader["home_teams"]);
                    string awayTeams = Convert.ToString(reader["home_teams"]);

                    divisionTeams.Teams = homeTeams.Split(",").ToList();
                    divisionTeams.Teams = divisionTeams.Teams.Concat(awayTeams.Split(",").ToList());
                    divisionTeamsList.AddLast(divisionTeams);
                }

                return divisionTeamsList;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public LinkedList<FootballDivisionTeams> GetActiveDivisionTeams(IEnumerable<String> activeDivisions)
        {
            //Select SQL Query
            string getSQL = "SELECT division, string_agg(home_team, ',') as home_teams, string_agg(away_team, ',') as away_teams FROM exercise.football_matches ";

            //Determine if a dynamic SQL query must be built
            if (activeDivisions.Count() > 0)
            {
                getSQL += "WHERE ";

                //Build dynamic SQL query
                for (int i = 0; i < activeDivisions.Count(); i++)
                {

                    getSQL += "division = '" + activeDivisions.ElementAt(i) + "'";

                    if (i != activeDivisions.Count() - 1)
                    {
                        getSQL += " OR ";
                    }
                }
            }

            //Complete Query
            getSQL += " GROUP BY division;";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballDivisionTeams>> readerFunction = (reader) =>
            {
                LinkedList<FootballDivisionTeams> divisionTeamsList = new LinkedList<FootballDivisionTeams>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballDivisionTeams divisionTeams = new FootballDivisionTeams();

                    divisionTeams.Division = Convert.ToString(reader["division"]);
                    string homeTeams = Convert.ToString(reader["home_teams"]);
                    string awayTeams = Convert.ToString(reader["home_teams"]);

                    divisionTeams.Teams = homeTeams.Split(",").ToList();
                    divisionTeams.Teams = divisionTeams.Teams.Concat(awayTeams.Split(",").ToList());
                    divisionTeamsList.AddLast(divisionTeams);
                }

                return divisionTeamsList;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }
    }
}
