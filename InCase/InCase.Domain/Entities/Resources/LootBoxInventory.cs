using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class LootBoxInventory : BaseEntity
    {
        public int NumberItems { get; set; }
        public int ChanceWining { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }
        public LootBox? Box { get; set; }
        public GameItem? Item { get; set; }

        public LootBoxInventoryDto Convert() => new()
        {
            NumberItems = NumberItems,
            ChanceWining = ChanceWining,
            ItemId = Item?.Id ?? ItemId,
            BoxId = Box?.Id ?? BoxId
        };
    }
}
