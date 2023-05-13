using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        public List<GameItem>? Items { get; set; }
        public List<LootBox>? Boxes { get; set; }
        public List<GameMarket>? Markets { get; set; }
        [JsonIgnore]
        public List<LootBoxGroup>? Groups { get; set; }
    }
}
