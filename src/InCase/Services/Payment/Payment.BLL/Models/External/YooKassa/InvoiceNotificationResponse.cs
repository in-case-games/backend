using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External.YooKassa;

public class InvoiceNotificationResponse
{
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("event")] public string? Event { get; set; }
    [JsonPropertyName("object")] public InvoiceCreateResponse? Object { get; set; }
}
