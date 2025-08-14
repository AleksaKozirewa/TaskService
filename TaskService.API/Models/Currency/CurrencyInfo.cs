using System.Text.Json.Serialization;

namespace TaskService.API.Models.Currency
{
    public class CurrencyInfo
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }

        [JsonPropertyName("NumCode")]
        public string NumCode { get; set; }

        [JsonPropertyName("CharCode")]
        public string CharCode { get; set; } // Например: USD, EUR

        [JsonPropertyName("Nominal")]
        public int Nominal { get; set; } // Номинал (например, 1 USD или 10 CNY)

        [JsonPropertyName("Name")]
        public string Name { get; set; } // "Доллар США", "Евро"

        [JsonPropertyName("Value")]
        public decimal Value { get; set; } // Курс в рублях

        [JsonPropertyName("Previous")]
        public decimal Previous { get; set; } // Предыдущее значение
    }
}
