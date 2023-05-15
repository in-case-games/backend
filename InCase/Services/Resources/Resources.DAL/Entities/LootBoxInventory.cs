using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class LootBoxInventory : BaseEntity
    {
        public GameItem? Item { get; set; }

        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }
        [JsonIgnore]
        public LootBox? Box { get; set; }
    }
}
