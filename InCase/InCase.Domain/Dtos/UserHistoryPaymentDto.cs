using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserHistoryPaymentDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public Guid UserId { get; set; }

        public UserHistoryPayment Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            Amount = Amount,
            UserId = UserId
        };
    }
}
