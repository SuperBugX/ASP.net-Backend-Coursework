namespace FootballAPI.Models.Football
{
    public class FootballDivisionModifier
    {
        //Attributes
        public string Division { get; set; }
        public string Status { get; set; }

        //Methods
        public FootballDivisionModifier(string division, string status)
        {
            Division = division;
            Status = status;
        }

        public FootballDivisionModifier()
        {
            Division = "";
            Status = "";
        }

        public override bool Equals(object? obj)
        {
            return obj is FootballDivisionModifier modifier &&
                   Division == modifier.Division &&
                   Status == modifier.Status;
        }
    }
}
