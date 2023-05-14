using System.Text.Json.Serialization;

namespace Payment.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<UserHistoryPayment>? HistoryPayments { get; set; }
    }
}
