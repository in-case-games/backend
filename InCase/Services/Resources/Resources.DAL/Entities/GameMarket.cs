using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class GameMarket : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
        [JsonIgnore]
        public List<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
    }
}
