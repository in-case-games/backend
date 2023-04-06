using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class LootBoxInventoryDto : BaseEntity
    {
        public int NumberItems { get; set; }
        public int ChanceWining { get; set; }

        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }

        public LootBoxInventory Convert() => new()
        {
            NumberItems = NumberItems,
            ChanceWining = ChanceWining,
            ItemId = ItemId,
            BoxId = BoxId
        };
    }
}
