using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External;
public class InvoiceCreateRequest
{
    [JsonPropertyName("amount")] public Amount? Amount { get; set; }
    [JsonPropertyName("capture")] public bool Capture { get; set; } = true;
    [JsonPropertyName("confirmation")] public Confirmation? Confirmation { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; } = "InCase.games";
    [JsonPropertyName("metadata")] public InvoiceCreateMetadata? Metadata { get; set; }
}

