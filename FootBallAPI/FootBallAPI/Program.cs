using AspNetCoreRateLimit;
using FootballAPI.Database;
using FootballAPI.Interfaces.CacheManager;
using FootballAPI.Interfaces.Database;
using FootballAPI.Interfaces.DataLayer;
using FootballAPI.Interfaces.Services;
using FootballAPI.Interfaces.Services.ServiceOrchistrators;
using FootballAPI.Repositories.Football;
using FootballAPI.ServiceOrchestrators;
using FootballAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Configure Swagger to expect the usage of authorisation via JWT Tokens
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "External API", Version = "v1" });

    #region SwaggerBearerSupport
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
    #endregion SwaggerBearerSupport
});

#region RequestThrottling
//Request Throttling Code
// needed to load configuration from appsettings.json
builder.Services.AddOptions();
// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();
//load general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
//load ip rules from appsettings.json
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
// inject counter and rules stores
builder.Services.AddInMemoryRateLimiting();
//Add framework services.
builder.Services.AddMvc();
#endregion RequestThrottling

#region DependencyInjection
//Service Dependency Injection
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<ICacheManager, MemoryCacheManager>();

//Services
builder.Services.AddTransient<IFootballModifiersService, FootballModifiersService>();
builder.Services.AddTransient<IFootballModifiersOrchestrator, FootballModifierOrchestrator>();
builder.Services.AddTransient<IFootballMatchService, FootballMatchService>();
builder.Services.AddTransient<IFootballStatisticsService, FootballStatisticsService>();
builder.Services.AddTransient<IAuthorisationService, AuthorisationService>();

//Repositories
builder.Services.AddTransient<IFootballMatchRepo, FootballMatchRepository>();
builder.Services.AddTransient<IFootballMatchStatisticsRepo, FootballStatisticsRepository>();

//Database
builder.Services.AddTransient<IDatabase, PostGresSQL>();
#endregion DependencyInjection

#region Authentication
//Authentication Scheme Code
//Get JWT security key from operating system environment variable or app settings
string securityKey = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY", EnvironmentVariableTarget.User) ?? builder.Configuration["JWT:Key"];

//Configure token validation
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],

    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:Audience"],

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
        Convert.FromBase64String(securityKey)),
};

//Set authentication scheme for JWT tokens bearer scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(opt =>
     {
         opt.ClaimsIssuer = tokenValidationParameters.ValidIssuer;
         opt.SaveToken = true;
         opt.TokenValidationParameters = tokenValidationParameters;
     });
#endregion Authentication

#region Initialisation
//Initialisation service for initial code execution
builder.Services.AddHostedService<InitialisationService>();
#endregion Initialisation

var app = builder.Build();

#region CORS
//Global CORS policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
#endregion CORS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
