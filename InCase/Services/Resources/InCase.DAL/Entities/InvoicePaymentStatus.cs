using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class InvoicePaymentStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserHistoryPayment? HistoryPayment { get; set; }
    }
}
