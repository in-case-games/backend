using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
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
