using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class UserHistoryPayment : BaseEntity
    {
        public string? InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }

        public PaymentInvoiceStatus? Status { get; set; }

        [JsonIgnore]
        public Guid StatusId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
