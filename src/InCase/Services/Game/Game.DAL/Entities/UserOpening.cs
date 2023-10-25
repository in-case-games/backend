using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class UserOpening : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public GameItem? Item { get; set; }
        public LootBox? Box { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }
    }
}
