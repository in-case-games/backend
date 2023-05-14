using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class UserInventory : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal FixedCost { get; set; }

        public User? User { get; set; }
        public GameItem? Item { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid ItemId { get; set; }
    }
}
