using InCase.Domain.Entities;

namespace InCase.Domain.Dtos
{
    public class LootBoxInventoryDto : BaseEntity
    {
        public int NumberItems { get; set; }
        public int ChanceWining { get; set; }

        public Guid? QualityId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }

        public LootBoxInventory Convert() => new()
        {
            NumberItems = NumberItems,
            ChanceWining = ChanceWining,
            QualityId = QualityId,
            ItemId = ItemId,
            BoxId = BoxId
        };
    }
}
