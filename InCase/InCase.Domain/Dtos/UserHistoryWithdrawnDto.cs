using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserHistoryWithdrawnDto : BaseEntity
    {
        public DateTime Date { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }

        public UserHistoryWithdrawn Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            UserId = UserId,
            ItemId = ItemId
        };
    }
}
