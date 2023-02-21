using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.External
{
    public class InvoiceAnswerStatusGM
    {
        [JsonPropertyName("user")]
        public Guid UserId { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("project")]
        public int ProjectId { get; set; }
        [JsonPropertyName("invoice")]
        public int InvoiceId { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("net_amount")]
        public decimal AmountUser { get; set; }
        [JsonPropertyName("received_amount")]
        public decimal ReceivedAmount { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("wallet")]
        public string? Wallet { get; set; }
        [JsonPropertyName("time")]
        public int Time { get; set; }
        [JsonPropertyName("currency_project")]
        public string? CurrencyProject { get; set; }
        [JsonPropertyName("currency_user")]
        public string? CurrencyUser { get; set; }
        [JsonPropertyName("date_create")]
        public DateTime DateCreate { get; set; }
        [JsonPropertyName("date_pay")]
        public DateTime DatePay { get; set; }
        [JsonPropertyName("rate")]
        public decimal? Rate { get; set; }
        [JsonPropertyName("signature")]
        public string? SignatureRSA { get; set; }

        public override string ToString()
        {
            return $"user:{UserId}";
        }
    }
}
