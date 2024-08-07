namespace FootballAPI.Models.API
{
    public class APIResponse
    {
        //Attributes
        public int Status { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }

        //Methods
        public APIResponse(int status, string error, object data)
        {
            Status = status;
            Error = error;
            Data = data;
        }

        public APIResponse()
        {
            Status = 200;
            Error = null;
            Data = null;
        }

        public override bool Equals(object? obj)
        {
            return obj is APIResponse response &&
                   Status == response.Status &&
                   Error == response.Error &&
                   EqualityComparer<object>.Default.Equals(Data, response.Data);
        }
    }
}
