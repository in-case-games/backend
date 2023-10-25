using System.Text.Json.Serialization;

namespace Authentication.DAL.Entities
{
    public class UserRestriction : BaseEntity
    {
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
