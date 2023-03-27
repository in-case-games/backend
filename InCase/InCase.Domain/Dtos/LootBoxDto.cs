using InCase.Domain.Entities;

namespace InCase.Domain.Dtos
{
    public class LootBoxDto : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Balance { get; set; } = 0;
        public decimal VirtualBalance { get; set; } = 0;
        public string? Image { get; set; } = "";
        public bool IsLocked { get; set; } = false;

        public Guid GameId { get; set; }

        public LootBox Convert() => new()
        {
            Name = Name,
            Cost = Cost,
            Balance = Balance,
            VirtualBalance = VirtualBalance,
            Image = Image,
            IsLocked = IsLocked,
            GameId = GameId
        };
    }
}
