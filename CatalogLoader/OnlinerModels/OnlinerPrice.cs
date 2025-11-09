using System.Text.Json.Serialization;

namespace CatalogLoader.OnlinerModels
{
    public class OnlinerPrice
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = "0";
    }
}