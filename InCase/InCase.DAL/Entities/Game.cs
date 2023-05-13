using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        public IEnumerable<GameItem>? Items { get; set; }
        public IEnumerable<LootBox>? Boxes { get; set; }
        public IEnumerable<GameMarket>? Markets { get; set; }
        [JsonIgnore]
        public IEnumerable<LootBoxGroup>? Groups { get; set; }
    }
}
