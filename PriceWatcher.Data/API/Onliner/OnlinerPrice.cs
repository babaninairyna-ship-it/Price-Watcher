using System.Text.Json.Serialization;

namespace PriceWatcher.Data.API.Onliner
{
    public class OnlinerPrice
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = "0";
    }
}