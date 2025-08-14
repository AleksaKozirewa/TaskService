using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
                _logger.LogError("Не задан API URL для получения курсов валют.");
                return StatusCode(500, "Ошибка конфигурации сервиса валют.");
            }

            if (_cache.TryGetValue(cacheKey, out CurrencyRates cachedRates))
            {
                _logger.LogInformation("Возвращаем кэшированные курсы валют (время кэширования: {CacheTime} минут).", cacheTimeMinutes);
                return Ok(cachedRates);
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                _logger.LogInformation("Запрашиваем актуальные курсы валют с {ApiUrl}", apiUrl);
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var rates = JsonSerializer.Deserialize<CurrencyRates>(content);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTimeMinutes));

                _cache.Set(cacheKey, rates, cacheOptions);
                _logger.LogInformation("Получены новые курсы валют и сохранены в кэш на {CacheTime} минут.", cacheTimeMinutes);

                return Ok(rates);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Ошибка HTTP при запросе курсов валют с {ApiUrl}.", apiUrl);
                return StatusCode(502, "Не удалось подключиться к сервису валют.");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Ошибка парсинга ответа с курсами валют.");
                return StatusCode(502, "Получены некорректные данные о валютах.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Непредвиденная ошибка при получении курсов валют.");
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }
    }
}
