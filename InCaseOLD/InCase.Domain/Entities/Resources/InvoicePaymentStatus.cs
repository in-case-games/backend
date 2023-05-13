using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class InvoicePaymentStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserHistoryPayment? HistoryPayment { get; set; }
    }
}
