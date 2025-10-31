using System.Text.Json.Serialization;

public class AmountInfo
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; } = "0";
}