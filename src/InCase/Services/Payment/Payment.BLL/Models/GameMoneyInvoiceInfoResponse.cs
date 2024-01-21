using Payment.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Payment.BLL.Models;

public class GameMoneyInvoiceInfoResponse : IGameMoneyResponse
{
    [JsonPropertyName("user")] public Guid UserId { get; set; }
    [JsonPropertyName("state")] public string? State { get; set; }
    [JsonPropertyName("project")] public string? ProjectId { get; set; }
    [JsonPropertyName("invoice")] public string? InvoiceId { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("amount")] public decimal Amount { get; set; }
    [JsonPropertyName("net_amount")] public decimal AmountUser { get; set; }
    [JsonPropertyName("received_amount")] public decimal ReceivedAmount { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("wallet")] public string? Wallet { get; set; }
    [JsonPropertyName("comment")] public string? Comment { get; set; }
    [JsonPropertyName("time")] public long Time { get; set; }
    [JsonPropertyName("currency_project")] public string? CurrencyProject { get; set; }
    [JsonPropertyName("currency_user")] public string? CurrencyUser { get; set; }
    [JsonPropertyName("date_create")] public DateTime DateCreate { get; set; }
    [JsonPropertyName("date_pay")] public DateTime DatePay { get; set; }
    [JsonPropertyName("rate")] public decimal Rate { get; set; } = 0M;
    [JsonPropertyName("rand")] public string? Rand { get; set; }
    [JsonPropertyName("reason")] public string? Reason { get; set; }
    [JsonPropertyName("signature")] public string SignatureRsa { get; set; } = null!;

    public override string ToString()
    {
        var rand = string.IsNullOrEmpty(Rand) ? "" : $"rand:{Rand};";
        var reason = string.IsNullOrEmpty(Reason) ? "" : $"reason:{Reason};";
        var comment = string.IsNullOrEmpty(Comment) ? "" : $"comment:{Comment};";
        var rate = Rate == 0 ? "" : $"rate:{Rate};";

        return
            $"state:{State};" +
            $"project:{ProjectId};" +
            $"invoice:{InvoiceId};" +
            $"status:{Status};" +
            $"amount:{Amount};" +
            $"net_amount:{AmountUser};" +
            $"recieved_amount:{ReceivedAmount};" +
            $"user:{UserId};" +
            $"type:{Type};" +
            $"wallet:{Wallet};" +
            $"{comment}" +
            $"time:{Time};" +
            $"currency_project:{CurrencyProject};" +
            $"currency_user:{CurrencyUser};" +
            $"date_create:{DateCreate};" +
            $"date_pay:{DatePay};" +
            $"{rate}" +
            $"{rand}" +
            $"{reason}";
    }
}