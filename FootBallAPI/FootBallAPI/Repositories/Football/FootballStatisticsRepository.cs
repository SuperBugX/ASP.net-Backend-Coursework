using FootballAPI.Interfaces.Database;
using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Models.DAO;
using System.Data.Common;

namespace FootballAPI.Repositories.Football
{
    public class FootballStatisticsRepository : IRepository<FootballMatchStatisticsDao>, IFootballMatchStatisticsRepo
    {
        //Attributes
        private static IDatabase _database;

        //Methods
        public FootballStatisticsRepository(IDatabase database)
        {
            _database = database;
        }

        public LinkedList<FootballMatchStatisticsDao> All()
        {
            //Select SQL Query
            string getSQL = "SELECT * FROM exercise.football_stats;";

            //Delegate function to read result rows
            Func<DbDataReader, LinkedList<FootballMatchStatisticsDao>> readerFunction = (reader) =>
            {
                LinkedList<FootballMatchStatisticsDao> statistics = new LinkedList<FootballMatchStatisticsDao>();

                //Loop records and create objects
                while (reader.Read())
                {
                    FootballMatchStatisticsDao statistic = new FootballMatchStatisticsDao();
                    statistic.Id = Convert.ToInt64(reader["id"]);
                    statistic.MatchId = Convert.ToInt64(reader["football_match_fk"]);
                    statistic.Attendance = Convert.ToInt32(reader["attendance"]);
                    statistic.Referee = Convert.ToString(reader["referee"]);
                    statistics.AddLast(statistic);
                }

                return statistics;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public FootballMatchStatisticsDao? Get(long id)
        {
            //Select SQl Query
            string getSQL = "SELECT * FROM exercise.football_stats WHERE id=" + id + ";";

            //Delegate function to read result rows
            Func<DbDataReader, FootballMatchStatisticsDao?> readerFunction = (reader) =>
            {
                FootballMatchStatisticsDao statistic = null;

                //Loop records and create objects
                while (reader.Read())
                {
                    statistic = new FootballMatchStatisticsDao();
                    statistic.Id = id;
                    statistic.MatchId = Convert.ToInt64(reader["football_match_fk"]);
                    statistic.Attendance = Convert.ToInt32(reader["attendance"]);
                    statistic.Referee = Convert.ToString(reader["referee"]);

                }

                return statistic;
            };

            //Execute SQL Query and get result
            return _database.ExecuteDataReader(getSQL, readerFunction);
        }

        public long Add(FootballMatchStatisticsDao entity)
        {
            //Insert SQL Query
            string insertSQL;

            //KeyValuePairs List
            LinkedList<KeyValuePair<string, object>> values = new LinkedList<KeyValuePair<string, object>>();
            values.AddFirst(new KeyValuePair<string, object>("attendance", entity.Attendance));
            values.AddFirst(new KeyValuePair<string, object>("referee", entity.Referee));

            //Adapt Insert SQL Query and List based on if the FootballStatistics object has a ID relating to a football match record
            if (entity.MatchId == -1)
            {
                insertSQL = "INSERT INTO exercise.football_stats(attendance, referee) VALUES(@attendance, @referee) RETURNING id;";
            }
            else
            {
                insertSQL = "INSERT INTO exercise.football_stats(football_match_fk, attendance, referee) VALUES(@football_match_fk, @attendance, @referee) RETURNING id;";
                values.AddFirst(new KeyValuePair<string, object>("football_match_fk", entity.MatchId));
            }

            //Execute SQL Query and get result
            return _database.ExecutePreparedStatement(insertSQL, values);
        }

        public bool DeleteAll()
        {
            //Delete All SQL Query
            string deleteSQL = "TRUNCATE exercise.football_stats RESTART IDENTITY CASCADE; ALTER SEQUENCE exercise.football_stats_id_seq RESTART WITH 1;";
            return _database.ExecuteNonQuery(deleteSQL) > 0;
        }
        public bool Delete(long id)
        {
            //Delete SQL Query
            string deleteSQL = "DELETE FROM exercise.football_stats WHERE id = " + id + ";";
            return _database.ExecuteNonQuery(deleteSQL) > 0;

        }

        public bool Update(long id, FootballMatchStatisticsDao entity)
        {
            //Update SQL Query
            string updateSQL = "UPDATE exercise.football_stats SET football_match_fk= @football_match_fk, attendance= @attendance, referee= @referee WHERE id= @id RETURNING *;";

            //KeyValuePairs List
            LinkedList<KeyValuePair<string, object>> values = new LinkedList<KeyValuePair<string, object>>();
            values.AddFirst(new KeyValuePair<string, object>("id", id));
            values.AddFirst(new KeyValuePair<string, object>("football_match_fk", entity.MatchId));
            values.AddFirst(new KeyValuePair<string, object>("attendance", entity.Attendance));
            values.AddFirst(new KeyValuePair<string, object>("referee", entity.Referee));

            //Execute SQL Query and get result
            return _database.ExecutePreparedStatement(updateSQL, values) != -1;
        }
    }
}
