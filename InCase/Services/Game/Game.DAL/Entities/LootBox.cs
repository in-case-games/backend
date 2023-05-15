using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class LootBox : BaseEntity
    {
        public decimal Cost { get; set; }
        public decimal Balance { get; set; } = 0;
        public decimal VirtualBalance { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public bool IsActiveBanner { get; set; } = false;

        public IEnumerable<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public IEnumerable<UserPathBanner>? Paths { get; set; }
    }
}
