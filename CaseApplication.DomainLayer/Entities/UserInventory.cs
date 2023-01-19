using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class UserInventory : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GameItemId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public GameItem? GameItem { get; set; }
    }
}
