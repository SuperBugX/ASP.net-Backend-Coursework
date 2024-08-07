namespace FootballAPI.Models.Football
{
    public class FootballDivisionTeams
    {
        //Attributes
        public string Division { get; set; }
        public IEnumerable<string> Teams { get; set; }

        //Methods
        public FootballDivisionTeams(string division, IEnumerable<string> teams)
        {
            Division = division;
            Teams = teams;
        }

        public FootballDivisionTeams()
        {
            Division = "";
            Teams = new LinkedList<string>();
        }

        public override bool Equals(object? obj)
        {
            return obj is FootballDivisionTeams teams &&
                   Division == teams.Division &&
                   EqualityComparer<IEnumerable<string>>.Default.Equals(Teams, teams.Teams);
        }
    }
}
