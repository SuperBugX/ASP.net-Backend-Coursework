using FootballAPI.Configuration;
using FootballAPI.Models.Responses.ModifiersService;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Net.Http.Headers;

namespace FootballAPI.API
{
    public class FootballModifiersAPI
    {
        //Attributes
        private const string URL = "https://be-2021-cw2-external-api.herokuapp.com/v1/modifiers/active";
        private const string TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwicm9sZXMiOiJEZXZlbG9wZXIiLCJleHAiOjE2NjM2ODY4Nzh9.ANe1bsE4SALD98aH_YDsC90At-iJ4lFV-CiNKQ2q_40";
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitPolicy;

        //Methods 
        public FootballModifiersAPI()
        {
            _retryPolicy = PollyPoliciesConfig.Instance.ModifiersAPIRetryPolicy;
            _circuitPolicy = PollyPoliciesConfig.Instance.ModifiersAPICircuitPolicy;
        }

        public async Task<Dictionary<string, string>> GetDivisionModifiersAsync()
        {
            Dictionary<string, string> divisionModifiersMap = null;

            await _retryPolicy.WrapAsync(_circuitPolicy).ExecuteAndCaptureAsync(async () =>
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    //Create and send a HTTP request to get the latest division modifiers
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                    var httpRequest = new HttpRequestMessage(new HttpMethod("GET"), URL);
                    var response = await httpClient.SendAsync(httpRequest);

                    //Check if the request was succesffuly
                    if (response.IsSuccessStatusCode)
                    {
                        //Convert HTTP repsonse into DivisionModifiersResponse object
                        DivisionModifiersResponse modifiersResponse = JsonConvert.DeserializeObject<DivisionModifiersResponse>(response.Content.ReadAsStringAsync().Result);

                        //Convert LinkedList of FootballDivisionModifier objects into Dictionary data struture equivalent
                        divisionModifiersMap = new Dictionary<string, string>();
                        Parallel.ForEach(modifiersResponse.Modifiers, footballDivisionModifier =>
                        {
                            divisionModifiersMap.Add(footballDivisionModifier.Division, footballDivisionModifier.Status);
                        });
                    }
                    else
                    {
                        //Throw request exception for retry
                        throw new HttpRequestException();
                    }
                }
            });

            return divisionModifiersMap;
        }
    }
}
