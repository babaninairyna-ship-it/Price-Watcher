using System.Text.Json.Serialization;

namespace CatalogLoader.OnlinerModels
{
    public class OnlinerPriceBlock
    {
        [JsonPropertyName("price_min")]
        public OnlinerPrice? PriceMin { get; set; }

        [JsonPropertyName("price_max")]
        public OnlinerPrice? PriceMax { get; set; }
    }
}