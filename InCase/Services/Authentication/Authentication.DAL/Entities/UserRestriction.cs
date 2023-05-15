using System.Text.Json.Serialization;

namespace Authentication.DAL.Entities
{
    public class UserRestriction : BaseEntity
    {
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid? OwnerId { get; set; }
        [JsonIgnore]
        public Guid TypeId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public User? Owner { get; set; }
        [JsonIgnore]
        public RestrictionType? Type { get; set; }
    }
}
