using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class UserHistoryPayment : BaseEntity
    {
        public string? InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }

        [JsonIgnore]
        public Guid StatusId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public InvoicePaymentStatus? Status { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public UserHistoryPaymentDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Currency = Currency,
            Rate = Rate,
            Date = Date,
            Amount = Amount,
            StatusId = Status?.Id ?? StatusId,
            UserId = User?.Id ?? UserId
        };
    }
}
