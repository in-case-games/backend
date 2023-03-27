using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class UserHistoryPaymentDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public Guid UserId { get; set; }

        public UserHistoryPayment Convert() => new()
        {
            Date = Date,
            Amount = Amount,
            UserId = UserId
        };
    }
}
