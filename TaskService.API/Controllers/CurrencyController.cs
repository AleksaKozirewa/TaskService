using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TaskService.API.Models.Currency;

namespace TaskService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CurrencyController> _logger;
        private readonly IConfiguration _configuration;

        public CurrencyController(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache,
            ILogger<CurrencyController> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("rates")]
        public async Task<ActionResult<CurrencyRates>> GetCurrencyRates()
        {
            const string cacheKey = "currency_rates";

            var apiUrl = _configuration["CurrencyApi:Url"];
            var cacheTimeMinutes = _configuration.GetValue<int>("CurrencyApi:CacheTimeMinutes");

            if (string.IsNullOrEmpty(apiUrl))
            {
                _logger.LogError("Currency API URL is not configured");
                return StatusCode(500, "Currency service configuration error");
            }

            if (_cache.TryGetValue(cacheKey, out CurrencyRates cachedRates))
            {
                _logger.LogInformation("Returning cached currency rates (Cache duration: {CacheTime} minutes)", cacheTimeMinutes);
                return Ok(cachedRates);
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                _logger.LogInformation("Fetching currency rates from {ApiUrl}", apiUrl);
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var rates = JsonSerializer.Deserialize<CurrencyRates>(content);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTimeMinutes));

                _cache.Set(cacheKey, rates, cacheOptions);
                _logger.LogInformation("Fetched fresh currency rates and cached for {CacheTime} minutes", cacheTimeMinutes);

                return Ok(rates);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while fetching currency rates from {ApiUrl}", apiUrl);
                return StatusCode(502, "Unable to connect to currency service");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Error parsing currency rates response");
                return StatusCode(502, "Invalid currency data received");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching currency rates");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
