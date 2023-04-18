using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class LootBoxInventoryDto : BaseEntity
    {
        public int ChanceWining { get; set; }

        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }

        public LootBoxInventory Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            ChanceWining = ChanceWining,
            ItemId = ItemId,
            BoxId = BoxId
        };
    }
}
