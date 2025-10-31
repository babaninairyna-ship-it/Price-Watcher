using System.Text.Json.Serialization;

public class OnlinerProduct
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("prices")]
    public PriceInfo Prices { get; set; } = new();
}