using System.Text.Json.Serialization;

namespace Identity.DAL.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public decimal Balance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
        public DateTime CreationDate { get; set; }
        public UserRole? Role { get; set; }

        [JsonIgnore]
        public Guid RoleId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
