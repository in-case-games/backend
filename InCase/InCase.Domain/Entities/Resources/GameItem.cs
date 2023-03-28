using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class GameItem : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public string? Image { get; set; }
        public string? IdForPlatform { get; set; }

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
        public List<UserHistoryWithdrawn>? HistoryWithdrawns { get; set; }
        [JsonIgnore]
        public List<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public List<UserInventory>? UserInventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public List<UserPathBanner>? PathBanners { get; set; }

        public GameItemDto Convert() => new()
        {
            Name = Name,
            Cost = Cost,
            Image = Image,
            IdForPlatform = IdForPlatform,
            TypeId = Type?.Id ?? TypeId,
            RarityId = Rarity?.Id ?? RarityId,
            GameId = Game?.Id ?? GameId,
            QualityId = Quality?.Id ?? QualityId
        };
    }
}
