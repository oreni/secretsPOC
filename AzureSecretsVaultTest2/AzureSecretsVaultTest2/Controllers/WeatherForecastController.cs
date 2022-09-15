using AzureSecretsVaultTest2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Dynamic;

namespace AzureSecretsVaultTest2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly OktaSettings _oktaSettingsObject;
        private readonly IOktaClient _oktaClient;
        private readonly IOktaSingletonClient _oktaSingletonClient;
        private readonly IOktaClientBad _oktaClientBad;
        private readonly IOktaClientWHttp _oktaClientWHttp;
        private readonly HttpClient _httpClientFromFactory;
        private readonly DbContext _dbContext;


        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            OktaSettings oktaSettingsObject,
            IOktaClientBad oktaClientBad,
            IOktaClient oktaClient,
            IOktaClientWHttp oktaClientWHttp,
            IHttpClientFactory httpClientFactory,
            DbContext dbContext)
        {
            _logger = logger;
            _oktaSettingsObject = oktaSettingsObject;
            _oktaClient = oktaClient;
            _oktaClientBad = oktaClientBad;
            _oktaClientWHttp = oktaClientWHttp;
            _httpClientFromFactory = httpClientFactory.CreateClient("AccountService");
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            MyDynamic.oktaSettingsObject = _oktaSettingsObject;
            MyDynamic.oktaScopedClient = _oktaClient?.GetOktaSettings();
            MyDynamic.oktaSingletonClient = _oktaSingletonClient?.GetOktaSettings();
            MyDynamic.oktaClientBAD = _oktaClientBad?.GetOktaSettings();
            MyDynamic.oktaClientWHttp = _oktaClientWHttp?.GetOktaSettings().BaseAddress;
            MyDynamic.oktaSingletonClient = _oktaSingletonClient?.GetOktaSettings();
            MyDynamic.httpClientFromFactory = _httpClientFromFactory?.BaseAddress;
            MyDynamic.httpClientFromFactoryDefaultRequestHeaders = _httpClientFromFactory.DefaultRequestHeaders;
            MyDynamic.DbContextConnectionString = _dbContext.Database.GetConnectionString();
            return Ok(MyDynamic);
        }
    }
}