using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class PaymentStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserPayment? Payment { get; set; }
    }
}
