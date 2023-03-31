using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
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

        public UserAdditionalInfoDto Convert() => new()
        {
            Balance = Balance,
            ImageUri = ImageUri,
            IsNotifyEmail = IsNotifyEmail,
            IsGuestMode = IsGuestMode,
            IsConfirmed = IsConfirmed,
            CreationDate = CreationDate,
            DeletionDate = DeletionDate,
            RoleId = Role?.Id ?? RoleId,
            UserId = User?.Id ?? UserId
        };
    }
}
