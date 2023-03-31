using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserAdditionalInfoDto : BaseEntity
    {
        public decimal Balance { get; set; } = 0;
        public string? ImageUri { get; set; } = "";
        public bool IsNotifyEmail { get; set; } = false;
        public bool IsGuestMode { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }

        public UserAdditionalInfo Convert() => new()
        {
            Balance = Balance,
            ImageUri = ImageUri,
            IsNotifyEmail = IsNotifyEmail,
            IsGuestMode = IsGuestMode,
            IsConfirmed = IsConfirmed,
            CreationDate = CreationDate,
            RoleId = RoleId,
            UserId = UserId
        };
    }
}
