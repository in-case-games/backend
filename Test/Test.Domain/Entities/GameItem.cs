using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
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
        public Guid TypeId { get; set; }
        [JsonIgnore]
        public Guid RarityId { get; set; }

        public GameItemType? Type { get; set; }
        public GameItemRarity? Rarity { get; set; }

        [JsonIgnore]
        public Game? Game { get; set; }
        [JsonIgnore]
        public List<UserHistoryWithdrawn>? HistoryWithdrawns { get; set; }
        [JsonIgnore]
        public List<UserInventory>? Inventories { get; set; }
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
            TypeId = Type!.Id,
            RarityId = Rarity!.Id,
            GameId = Game?.Id ?? GameId,
        };
    }
}
