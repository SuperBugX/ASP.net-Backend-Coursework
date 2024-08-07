using FootballAPI.Exceptions.HTTP;
using FootballAPI.Interfaces.Services;
using FootballAPI.Interfaces.Services.ServiceOrchistrators;
using FootballAPI.Models.API;
using FootballAPI.Models.Football;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;

namespace FootballAPI.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = "developer,customer")]
    [ApiController]
    public class FootballAPIController : ControllerBase
    {
        //Attributes
        private IConfiguration _config;
        private IAuthorisationService _authorisationService;
        private IFootballMatchService _footballMatchService;
        private IFootballStatisticsService _footballStatisticsService;
        private IFootballModifiersService _footballModifiersService;
        private IFootballModifiersOrchestrator _footballModifiersOrchestrator;

        //Methods
        public FootballAPIController(IConfiguration config,
            IAuthorisationService authorisationService,
            IFootballMatchService footballMatchService,
            IFootballStatisticsService footballStatisticsService,
            IFootballModifiersService footballModifiersService,
            IFootballModifiersOrchestrator footballModifiersOrchestrator) : base()
        {
            _config = config;
            _authorisationService = authorisationService;
            _footballMatchService = footballMatchService;
            _footballStatisticsService = footballStatisticsService;
            _footballModifiersService = footballModifiersService;
            _footballModifiersOrchestrator = footballModifiersOrchestrator;
        }

        /*
            Method returns a new APIResponse based on a provided Exception
            The type of exception determines the exact HTTP status code and error message
            of the returned APIResponse.
            NOTE: No "Data" is returned within the APIResponse
         */
        private APIResponse GenerateAPIErrorResponse(Exception ex)
        {
            //Create default response
            APIResponse apiResponse = new APIResponse(0, ex.Message, "");

            //Adapt response HTTP status code and error message based on exception type
            if (ex is ArgumentException)
            {
                apiResponse.Status = 400;
            }
            else if (ex is MissingResourceException)
            {
                apiResponse.Status = 404;
            }
            else
            {
                apiResponse.Status = 500;
                apiResponse.Error = "Internal Operation Error";
            }

            //Return complete API Error Response
            return apiResponse;
        }

        //GET Methods
        [AllowAnonymous]
        [HttpGet]
        [Route("/authenticate/{apiKey}")]
        //Method returns a JWT Token upon successful API key authentication
        public IActionResult Authenticate(string apiKey)
        {
            APIResponse result;

            try
            {
                //Input Validation
                if (!String.IsNullOrEmpty(apiKey))
                {
                    //Get role from API key
                    string role = _authorisationService.GetRoleByAPIKey(apiKey);

                    //Validate role result
                    if (String.IsNullOrEmpty(role))
                    {
                        //Return invalid argument exception message to caller
                        result = new APIResponse(403, "Invalid API Key", "");
                        return new OkObjectResult(result);
                    }

                    //Add user role as claim
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Role, role)
                    };

                    //Create JWT Token
                    var token = new JwtSecurityToken
                        (
                            issuer: _config["JWT:Issuer"],
                            audience: _config["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(15),
                            notBefore: DateTime.UtcNow,
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(_config["JWT:Key"])), SecurityAlgorithms.HmacSha256)
                        );

                    //Serialise token
                    string stringToken = new JwtSecurityTokenHandler().WriteToken(token);

                    //Create and return API response
                    result = new APIResponse(200, "", stringToken);
                    return new OkObjectResult(result);
                }
                else
                {
                    //Return invalid argument message to caller
                    result = new APIResponse(400, "Missing API Key", "");
                    return new OkObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [Authorize(Roles = "developer")]
        [HttpGet("/football/match/division/modifiers")]
        //Method returns current football division modifiers
        public IActionResult GetAllDivisionModifiers()
        {
            APIResponse result;

            try
            {
                Dictionary<string, string> modifiers = _footballModifiersService.GetDivisionModifiersAsync().Result;
                //Create and return API response
                result = new APIResponse(200, "", modifiers);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [Authorize(Roles = "developer")]
        [HttpGet("/football/match")]
        //Method returns all FootBallMatch objects stored
        public IActionResult GetAllFootballMatches()
        {
            string result;

            try
            {
                //Get all matches in Enumerable
                LinkedList<FootballMatch> matches = _footballMatchService.GetAllMatches();
                //Create and return API response
                result = JsonConvert.SerializeObject(new APIResponse(200, "", matches));
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = JsonConvert.SerializeObject(GenerateAPIErrorResponse(ex));
                return new OkObjectResult(result);
            }
        }

        [Authorize(Roles = "developer")]
        [HttpGet("/football/match/{id}")]
        //Method returns a single FootBallMatch object based on an ID
        public IActionResult GetFootballMatch(long id)
        {
            APIResponse result;

            try
            {
                //Get specific match by ID
                FootballMatch match = _footballMatchService.GetMatch(id);

                //Create and return API response
                result = new APIResponse(200, "", match);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [HttpGet("/football/match/byDate")]
        /*
            Method returns a number of FootballMatch objects found within a start and end date.

            NOTE: Atleast one DateTime object must be provided, the missing date argument is replaced 
            with a maximum or minimum date respectively.
        */
        public IActionResult GetFootballMatchesByDates(DateTime? startDate, DateTime? endDate)
        {
            APIResponse result;

            try
            {
                //Get a Enumerable of football matches within a date range
                LinkedList<FootballMatch> matches = _footballMatchService.GetMatchesByDates(startDate, endDate);

                //Create and return API response
                result = new APIResponse(200, "", matches);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [HttpGet("/football/match/byTeams")]
        /*
            Method returns a number of FootballMatch objects based on the 
            specified Home Team and Away Team names.

            NOTE: Atleast one team name must be provided, the missing team name is ignored.
        */
        public IActionResult GetFootballMatchesByTeams(string? homeTeam, string? awayTeam)
        {
            APIResponse result;

            //Check if string inputs are null and initialise an empty string
            if (String.IsNullOrEmpty(homeTeam))
            {
                homeTeam = "";
            }

            if (String.IsNullOrEmpty(awayTeam))
            {
                awayTeam = "";
            }

            try
            {
                //Get all football matches based on provided team names
                LinkedList<FootballMatch> matches = _footballMatchService.GetMatchesByTeams(homeTeam, awayTeam);

                //Create and return API response
                result = new APIResponse(200, "", matches);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }


        [HttpGet("/football/match/byTeamAndDate")]
        /*
            Method returns a number of FootballMatch objects based on the team name 
            provided and start and end date range.

            NOTE: The team name is both used for Home and Away football matches.
            Atleast one DateTime object must be provided, the missing date argument is replaced 
            with a maximum or minimum date respectively.
        */
        public IActionResult GetFootballMatchesByTeamAndDate(string team, DateTime startDate, DateTime endDate)
        {
            APIResponse result;

            try
            {
                //Get all football matches based on team name and date range
                LinkedList<FootballMatch> matches = _footballMatchService.GetMatchesByTeamAndDate(team, startDate, endDate);

                //Create and return API response
                result = new APIResponse(200, "", matches);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [HttpGet("/football/match/division")]
        /*
            Method returns a number of FootballDivisionTeams objects each representing
            a league division and their participating teams.

            NOTE:Only divisions which are currently "active" based on an external API are returned
        */
        public IActionResult GetActiveFootballDivisionTeams()
        {
            string result;

            try
            {
                LinkedList<FootballDivisionTeams> divisionTeams = _footballModifiersOrchestrator.GetActiveDivisionTeams();

                //Create and return API response
                result = JsonConvert.SerializeObject(new APIResponse(200, "", divisionTeams));
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = JsonConvert.SerializeObject(GenerateAPIErrorResponse(ex));
                return new OkObjectResult(result);
            }
        }

        //POST Methods
        [Authorize(Roles = "developer")]
        [HttpPost("/football/match")]
        /*
            Method saves a new FootballMatch object into storage.

            NOTE: If a FootballMatchStatistics object is present within 
            the argument, it is also inserted.
        */
        public IActionResult PostFootballMatch([FromBody] FootballMatch match)
        {
            APIResponse result;

            try
            {
                //Add football match to storage, Get new ID
                long entryId = _footballMatchService.AddMatch(match);

                //Create and return API response
                result = new APIResponse(200, "", entryId);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [Authorize(Roles = "developer")]
        [HttpPost("/football/statistic")]
        //Method saves a new FootballMatchStatistics object into storage
        public IActionResult PostFootballMatchStatistic([FromBody] FootballMatchStatistics statistic)
        {
            APIResponse result;

            try
            {
                //Add football match statistic to storage, Get new ID
                long entryId = _footballStatisticsService.AddStatistic(statistic);

                //Create and return API response
                result = new APIResponse(200, "", entryId);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        //DELETE Methods
        [Authorize(Roles = "developer")]
        [HttpDelete("/football/match/{id}")]
        /*
            Method deletes from storage a football match based on ID.

            NOTE: On successful deletion, any associated match statistics are also deleted.
        */
        public IActionResult DeleteFootballMatch(int id)
        {
            APIResponse result;

            try
            {
                //Delete football match based on ID
                Boolean hasDeleted = _footballMatchService.DeleteMatch(id);

                if (hasDeleted)
                {
                    //Create and return API response indicating successful deletion
                    result = new APIResponse(200, "", "Resource was deleted");
                    return new OkObjectResult(result);
                }
                else
                {
                    //Create and return API response indicating no changes occured
                    result = new APIResponse(204, "", "Resource did not exist");
                    return new OkObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [Authorize(Roles = "developer")]
        [HttpDelete("/football/statistic/{id}")]
        //Method deletes from storage a football match statistic based on ID
        public IActionResult DeleteFootballMatchStatistic(long id)
        {
            APIResponse result;

            try
            {
                //Delete football match statistic based on ID
                Boolean hasDeleted = _footballStatisticsService.DeleteStatistic(id);

                if (hasDeleted)
                {
                    //Create and return API response indicating successful deletion
                    result = new APIResponse(200, "", "Resource was deleted");
                    return new OkObjectResult(result);

                }
                else
                {
                    //Create and return API response indicating no changes occured
                    result = new APIResponse(204, "", "Resource did not exist");
                    return new OkObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        //PUT Methods
        [Authorize(Roles = "developer")]
        [HttpPut("/football/match/{id}")]
        /*
            Method updates a whole football match entry on storage based on the provided ID 
            and FootballMatch object provided.

            NOTE: Method does not update related football match statistics based on the 
            FootballMatch object provided.
        */
        public IActionResult PutFootballMatch(int id, [FromBody] FootballMatch match)
        {
            APIResponse result;

            try
            {
                Boolean hasUpdated = _footballMatchService.UpdateMatch(id, match);

                if (hasUpdated)
                {
                    //Create API response indicating success
                    result = new APIResponse(200, "", "");
                }
                else
                {
                    //Create API response indicating failure
                    result = new APIResponse(500, "Unable to update match", "");
                }

                //Return API response
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }

        [Authorize(Roles = "developer")]
        [HttpPut("/football/statistic/{id}")]
        /*
            Method updates a whole football match statistics entry on storage based on the
            provided ID and FootballMatchStatistic object provided.
        */
        public IActionResult PutFootballMatchStatistic(int id, [FromBody] FootballMatchStatistics stastitic)
        {
            APIResponse result;

            try
            {
                Boolean hasUpdated = _footballStatisticsService.UpdateStatistic(id, stastitic);

                if (hasUpdated)
                {
                    //Create API response indicating success
                    result = new APIResponse(200, "", "");
                }
                else
                {
                    //Create API response indicating failure
                    result = new APIResponse(500, "Unable to update statistic", "");
                }

                //Return API response
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //Handle exception and generate appropriate response
                result = GenerateAPIErrorResponse(ex);
                return new OkObjectResult(result);
            }
        }
    }
}