using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public IEnumerable<GameItem>? Items { get; set; }
        [JsonIgnore]
        public IEnumerable<GameMarket>? Markets { get; set; }
    }
}
