using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserHistoryPromocodeDto : BaseEntity
    {
        public DateTime? Date { get; set; }
        public bool IsActivated { get; set; } = false;

        public Guid UserId { get; set; }
        public Guid PromocodeId { get; set; }

        public UserHistoryPromocode Convert() => new()
        {
            Id = Id,
            Date = Date,
            IsActivated = IsActivated,
            UserId = UserId,
            PromocodeId = PromocodeId
        };
    }
}
