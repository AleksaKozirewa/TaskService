using System.Text.Json.Serialization;

namespace TaskService.API.Models.Currency
{
    public class CurrencyRates
    {
        [JsonPropertyName("Valute")]
        public Dictionary<string, CurrencyInfo> Valute { get; set; }
    }
}
