using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class UserPathBanner : BaseEntity
    {
        public DateTime Date { get; set; }
        public int NumberSteps { get; set; }
        public decimal FixedCost { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BannerId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }
        public LootBoxBanner? Banner { get; set; }

        public UserPathBannerDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            NumberSteps = NumberSteps,
            FixedCost = FixedCost,
            UserId = User?.Id ?? UserId,
            ItemId = Item?.Id ?? ItemId,
            BannerId = Banner?.Id ?? BannerId
        };
    }
}
