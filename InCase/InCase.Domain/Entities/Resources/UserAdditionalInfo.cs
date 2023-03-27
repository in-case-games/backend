using System.Text.Json.Serialization;
using InCase.Domain.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InCase.Domain.Entities.Resources
{
    public class UserAdditionalInfo : BaseEntity
    {
        public decimal Balance { get; set; } = 0;
        public string? Image { get; set; } = "";
        public bool IsNotifyEmail { get; set; } = false;
        public bool IsGuestMode { get; set; } = false;

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
            Image = Image,
            IsNotifyEmail = IsNotifyEmail,
            RoleId = Role?.Id ?? RoleId,
            UserId = User?.Id ?? UserId,
            IsGuestMode = IsGuestMode
        };
    }
}
