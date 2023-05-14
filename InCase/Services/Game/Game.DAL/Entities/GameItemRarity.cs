using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class GameItemRarity : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public GameItem? Item { get; set; }
    }
}
