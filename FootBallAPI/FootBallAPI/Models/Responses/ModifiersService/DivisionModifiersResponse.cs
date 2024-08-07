using FootballAPI.Models.Football;

namespace FootballAPI.Models.Responses.ModifiersService
{
    public class DivisionModifiersResponse
    {
        //Attributes
        public LinkedList<FootballDivisionModifier> Modifiers { get; set; }

        //Methods
        public DivisionModifiersResponse(LinkedList<FootballDivisionModifier> modifiers)
        {
            Modifiers = modifiers;
        }

        public DivisionModifiersResponse()
        {
            Modifiers = new LinkedList<FootballDivisionModifier>();
        }

        public override bool Equals(object? obj)
        {
            return obj is DivisionModifiersResponse response &&
                   EqualityComparer<LinkedList<FootballDivisionModifier>>.Default.Equals(Modifiers, response.Modifiers);
        }
    }
}
