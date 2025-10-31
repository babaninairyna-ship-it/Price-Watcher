using System.Text.Json.Serialization;

public class OnlinerResponse
{
    [JsonPropertyName("products")]
    public List<OnlinerProduct> Products { get; set; } = new();
}