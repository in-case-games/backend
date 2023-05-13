using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class LootBox : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Balance { get; set; } = 0;
        public decimal VirtualBalance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
        public bool IsLocked { get; set; } = false;

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
        public List<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public List<LootBoxGroup>? Groups { get; set; }
        [JsonIgnore]
        public LootBoxBanner? Banner { get; set; }
    }
}
