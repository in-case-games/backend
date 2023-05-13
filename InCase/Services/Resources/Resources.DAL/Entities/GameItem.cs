using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class GameItem : BaseEntity
    {
        public string? Name { get; set; }
        public string? HashName { get; set; }
        public decimal Cost { get; set; }
        public string? ImageUri { get; set; }
        public string? IdForMarket { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Guid? TypeId { get; set; }
        [JsonIgnore]
        public Guid? RarityId { get; set; }
        [JsonIgnore]
        public Guid? QualityId { get; set; }
        public GameItemQuality? Quality { get; set; }
        public GameItemType? Type { get; set; }
        public GameItemRarity? Rarity { get; set; }

        [JsonIgnore]
        public Game? Game { get; set; }
        [JsonIgnore]
        public List<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
        [JsonIgnore]
        public List<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public List<UserInventory>? UserInventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public List<UserPathBanner>? PathBanners { get; set; }
    }
}
