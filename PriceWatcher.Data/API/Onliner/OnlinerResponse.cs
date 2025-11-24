using System.Text.Json.Serialization;

namespace PriceWatcher.Data.API.Onliner
{
    public class OnlinerResponse
    {
        [JsonPropertyName("products")]
        public List<OnlinerProduct> Products { get; set; } = new List<OnlinerProduct>();
    }
}
