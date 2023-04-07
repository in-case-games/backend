using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class UserHistoryPayment : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public UserHistoryPaymentDto Convert() => new()
        {
            Id = Id,
            Date = Date,
            Amount = Amount,
            UserId = User?.Id ?? UserId
        };
    }
}
