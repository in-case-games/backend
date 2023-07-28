using System.Text.Json.Serialization;

namespace Authentication.DAL.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public bool IsConfirmed { get; set; } = false;
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
