using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserHistoryPaymentDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }

        public Guid StatusId { get; set; }
        public Guid UserId { get; set; }

        public UserHistoryPayment Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Currency = Currency,
            Rate = Rate,
            Date = Date,
            Amount = Amount,
            StatusId = StatusId,
            UserId = UserId
        };
    }
}
