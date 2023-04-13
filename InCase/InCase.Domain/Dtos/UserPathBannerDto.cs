using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserPathBannerDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public int NumberSteps { get; set; }
        public decimal FixedCost { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BannerId { get; set; }

        public UserPathBanner Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            NumberSteps = NumberSteps,
            FixedCost = FixedCost,
            UserId = UserId,
            ItemId = ItemId,
            BannerId = BannerId
        };
    }
}
