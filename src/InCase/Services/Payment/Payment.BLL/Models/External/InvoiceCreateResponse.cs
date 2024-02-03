using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External;

public class InvoiceCreateResponse
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("paid")] public bool Paid { get; set; }
    [JsonPropertyName("amount")] public Amount? Amount { get; set; }
    [JsonPropertyName("confirmation")] public Confirmation? Confirmation { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedDate { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("recipient")] public Recipient? Recipient { get; set; }
    [JsonPropertyName("refundable")] public bool Refundable { get; set; }
    [JsonPropertyName("test")] public bool Test { get; set; }
}