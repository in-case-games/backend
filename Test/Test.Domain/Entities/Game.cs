using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Test.Domain.Entities
{
    [Table("Game")]
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        public List<GameItem>? Items { get; set; }
        public List<LootBox>? Boxes { get; set; }
        public List<GamePlatform>? Platforms { get; set; }

        [JsonIgnore]
        public List<LootBoxGroup>? LootBoxGroups { get; set; }
    }
}
