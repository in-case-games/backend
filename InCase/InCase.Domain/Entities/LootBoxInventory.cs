using System.Text.Json.Serialization;

namespace InCase.Domain.Entities
{
    public class LootBoxInventory : BaseEntity
    {
        public int NumberItems { get; set; }
        public int ChanceWining { get; set; }

        [JsonIgnore]
        public Guid? QualityId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }

        public GameItemQuality? Quality { get; set; }
        public LootBox? Box { get; set; }
        public GameItem? Item { get; set; }

        public LootBoxInventory Convert() => new()
        {
            NumberItems = NumberItems,
            ChanceWining = ChanceWining,
            QualityId = Quality?.Id ?? QualityId,
            ItemId = Item?.Id ?? ItemId,
            BoxId = Box?.Id ?? BoxId
        };
    }
}
