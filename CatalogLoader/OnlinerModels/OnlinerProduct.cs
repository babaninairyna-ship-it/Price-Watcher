using System.Text.Json.Serialization;

namespace CatalogLoader.OnlinerModels
{
    public class OnlinerProduct
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("prices")]
        public OnlinerPriceBlock Prices { get; set; } = new OnlinerPriceBlock();

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

    }
}