using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class GameItem : BaseEntity
    {
        public string? Name { get; set; }
        public string? HashName { get; set; }
        public decimal Cost { get; set; }
        public string? IdForMarket { get; set; }

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
        public List<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
    }
}
