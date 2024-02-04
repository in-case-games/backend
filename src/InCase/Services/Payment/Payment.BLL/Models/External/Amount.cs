using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External;

public class Amount
{
    [JsonPropertyName("value")] public decimal Value { get; set; }
    [JsonPropertyName("currency")] public string? Currency { get; set; }
}