using System.Text.Json.Serialization;

namespace CatalogLoader.OnlinerModels
{
    public class OnlinerResponse
    {
        [JsonPropertyName("products")]
        public List<OnlinerProduct> Products { get; set; } = new List<OnlinerProduct>();
    }
}
