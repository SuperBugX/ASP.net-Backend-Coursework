using Npgsql;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Globalization;
using System.Net.Sockets;

namespace FootballAPI.Configuration
{
    public sealed class PollyPoliciesConfig
    {
        //Attributes
        private static PollyPoliciesConfig instance = null;
        private static readonly object padlock = new object();

        //Modifiers API Policies
        public AsyncRetryPolicy ModifiersAPIRetryPolicy { get; set; }
        public AsyncCircuitBreakerPolicy ModifiersAPICircuitPolicy { get; set; }

        //PostGresSQL Policies
        public AsyncCircuitBreakerPolicy PostGresSqlCircuitPolicy { get; set; }

        //Methods
        PollyPoliciesConfig()
        {
            //Modifiers API Policies
            ModifiersAPIRetryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(3));

            ModifiersAPICircuitPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMinutes(1));

            //PostGres Policies
            PostGresSqlCircuitPolicy = Policy
                .Handle<PostgresException>(ex => !ex.SqlState.StartsWith("42", false, CultureInfo.InvariantCulture))
                .OrInner<SocketException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMinutes(1));
        }

        //Method returns singleton instance
        public static PollyPoliciesConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PollyPoliciesConfig();
                    }

                    return instance;
                }
            }
        }

    }
}
