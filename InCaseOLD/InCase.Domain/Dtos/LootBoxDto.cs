using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class LootBoxDto : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public string? ImageUri { get; set; } = "";
        public bool IsLocked { get; set; } = false;

        public Guid GameId { get; set; }

        public LootBox Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Name = Name,
            Cost = Cost,
            ImageUri = ImageUri,
            IsLocked = IsLocked,
            GameId = GameId
        };
    }
}
