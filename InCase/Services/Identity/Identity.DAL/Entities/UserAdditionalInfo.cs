using System.Text.Json.Serialization;

namespace Identity.DAL.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public UserRole? Role { get; set; }

        [JsonIgnore]
        public Guid RoleId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
