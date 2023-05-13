using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class UserHistoryOpening : BaseEntity
    {
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [JsonIgnore]
        public Guid BoxId { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }
        public LootBox? Box { get; set; }
    }
}
