using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class LootBoxGroup : BaseEntity
    {
        public GroupLootBox? Group { get; set; }
        public LootBox? Box { get; set; }

        [JsonIgnore]
        public Guid BoxId { get; set; }
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
    }
}
