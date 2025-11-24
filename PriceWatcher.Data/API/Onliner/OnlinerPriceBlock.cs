using System.Text.Json.Serialization;

namespace PriceWatcher.Data.API.Onliner
{
    public class OnlinerPriceBlock
    {
        [JsonPropertyName("price_min")]
        public OnlinerPrice? PriceMin { get; set; }

        [JsonPropertyName("price_max")]
        public OnlinerPrice? PriceMax { get; set; }
    }
}