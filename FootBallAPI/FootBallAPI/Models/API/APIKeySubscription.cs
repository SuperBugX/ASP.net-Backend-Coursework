namespace FootballAPI.Models.API
{
    public class APIKeySubscription
    {
        //Attributes
        public long Id { get; set; }
        public string Key { get; set; }
        public string Role { get; set; }

        //Methods
        public APIKeySubscription(long id, string key, string role)
        {
            Id = id;
            Key = key;
            Role = role;
        }

        public override bool Equals(object? obj)
        {
            return obj is APIKeySubscription subscription &&
                   Id == subscription.Id &&
                   Key == subscription.Key &&
                   Role == subscription.Role;
        }
    }
}
