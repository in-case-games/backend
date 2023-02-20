using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Internal
{
    public class UserInventory : BaseEntity
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid GameItemId { get; set; }
        public DateTime? ExpiryTime { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public GameItem? GameItem { get; set; }
    }
}
