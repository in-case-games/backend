using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class UserHistoryWithdrawnDto : BaseEntity
    {
        public DateTime Date { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }

        public UserHistoryWithdrawn Convert() => new()
        {
            Date = Date,
            UserId = UserId,
            ItemId = ItemId
        };
    }
}
