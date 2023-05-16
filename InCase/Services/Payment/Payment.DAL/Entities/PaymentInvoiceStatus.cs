using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class PaymentInvoiceStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserPayments? Payments { get; set; }
    }
}
