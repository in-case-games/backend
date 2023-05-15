using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class GameItem : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }

        public GameItemQuality? Quality { get; set; }
        public GameItemType? Type { get; set; }
        public GameItemRarity? Rarity { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Guid? TypeId { get; set; }
        [JsonIgnore]
        public Guid? RarityId { get; set; }
        [JsonIgnore]
        public Guid? QualityId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
        [JsonIgnore]
        public IEnumerable<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public IEnumerable<UserInventory>? UserInventories { get; set; }
        [JsonIgnore]
        public IEnumerable<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public IEnumerable<UserPathBanner>? PathBanners { get; set; }
    }
}
