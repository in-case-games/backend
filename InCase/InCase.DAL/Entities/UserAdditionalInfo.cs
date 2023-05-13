using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public decimal Balance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
        public bool IsNotifyEmail { get; set; } = false;
        public bool IsGuestMode { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeletionDate { get; set; }

        [JsonIgnore]
        public Guid RoleId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }

        public UserRole? Role { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
