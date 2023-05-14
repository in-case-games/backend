using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class PaymentInvoiceStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserHistoryPayment? HistoryPayment { get; set; }
    }
}
