using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class LootBoxDto : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Balance { get; set; } = 0;
        public decimal VirtualBalance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
        public bool IsLocked { get; set; } = false;

        public Guid GameId { get; set; }

        public LootBox Convert() => new()
        {
            Name = Name,
            Cost = Cost,
            Balance = Balance,
            VirtualBalance = VirtualBalance,
            ImageUri = ImageUri,
            IsLocked = IsLocked,
            GameId = GameId
        };
    }
}
