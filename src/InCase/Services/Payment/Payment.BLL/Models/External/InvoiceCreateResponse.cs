using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External;

public class InvoiceCreateResponse
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("paid")] public bool Paid { get; set; }
    [JsonPropertyName("amount")] public Amount? Amount { get; set; }
    [JsonPropertyName("confirmation")] public Confirmation? Confirmation { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("recipient")] public Recipient? Recipient { get; set; }
    [JsonPropertyName("refundable")] public bool Refundable { get; set; }
    [JsonPropertyName("test")] public bool Test { get; set; }

    public override string ToString() => 
        $"Id - {Id}; " +
        $"Status - {Status}; " +
        $"Paid - {Paid}; " +
        $"Amount.Value - {Amount?.Value}; " +
        $"Amount.Currency - {Amount?.Currency}; " +
        $"Confirmation.Type - {Confirmation?.Type}; " +
        $"Confirmation.ReturnUrl - {Confirmation?.ReturnUrl}; " +
        $"Confirmation.ConfirmationUrl - {Confirmation?.ConfirmationUrl}; " +
        $"CreatedAt - {CreatedAt}; " +
        $"Description - {Description}; " +
        $"Recipient.AccountId - {Recipient?.AccountId}; " +
        $"Recipient.GatewayId - {Recipient?.GatewayId}; " +
        $"Refundable - {Refundable}; " +
        $"Test - {Test};";
}