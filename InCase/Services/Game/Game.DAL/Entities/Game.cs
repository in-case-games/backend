using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        public IEnumerable<GameItem>? Items { get; set; }
        public IEnumerable<LootBox>? Boxes { get; set; }

        [JsonIgnore]
        public IEnumerable<LootBoxGroup>? Groups { get; set; }
    }
}
