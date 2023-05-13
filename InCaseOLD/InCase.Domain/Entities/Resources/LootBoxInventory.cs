using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class LootBoxInventory : BaseEntity
    {
        public int ChanceWining { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }
        [JsonIgnore]
        public LootBox? Box { get; set; }
        public GameItem? Item { get; set; }

        public LootBoxInventoryDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            ChanceWining = ChanceWining,
            ItemId = Item?.Id ?? ItemId,
            BoxId = Box?.Id ?? BoxId
        };
    }
}
