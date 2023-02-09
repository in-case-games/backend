using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class UserInventory : BaseEntity
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid GameItemId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public GameItem? GameItem { get; set; }
    }
}
