using InCase.Domain.Entities;

namespace InCase.Domain.Dtos
{
    public class UserTokenDto : BaseEntity
    {
        public string? RefreshToken { get; set; }
        public string? EmailToken { get; set; }
        public string? IpAddress { get; set; }
        public string? Device { get; set; }

        public Guid UserId { get; set; }

        public UserToken Convert() => new()
        {
            RefreshToken = RefreshToken,
            EmailToken = EmailToken,
            IpAddress = IpAddress,
            Device = Device,

            UserId = UserId
        };
    }
}
