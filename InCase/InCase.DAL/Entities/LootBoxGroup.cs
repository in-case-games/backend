using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class LootBoxGroup : BaseEntity
    {
        [JsonIgnore]
        public Guid BoxId { get; set; }
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }

        public GroupLootBox? Group { get; set; }
        public LootBox? Box { get; set; }
        [JsonIgnore]
        public Game? Game { get; set; }
    }
}
