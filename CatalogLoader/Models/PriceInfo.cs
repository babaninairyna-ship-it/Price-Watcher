using System.Text.Json.Serialization;

public class PriceInfo
{
    [JsonPropertyName("price_min")]
    public AmountInfo PriceMin { get; set; } = new();

    [JsonPropertyName("price_max")]
    public AmountInfo PriceMax { get; set; } = new();
}