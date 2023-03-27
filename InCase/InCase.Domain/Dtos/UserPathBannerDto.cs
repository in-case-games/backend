using InCase.Domain.Entities;

namespace InCase.Domain.Dtos
{
    public class UserPathBannerDto : BaseEntity
    {
        public DateTime Date { get; set; }
        public int NumberSteps { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BannerId { get; set; }

        public UserPathBanner Convert() => new()
        {
            Date = Date,
            NumberSteps = NumberSteps,
            UserId = UserId,
            ItemId = ItemId,
            BannerId = BannerId
        };
    }
}
