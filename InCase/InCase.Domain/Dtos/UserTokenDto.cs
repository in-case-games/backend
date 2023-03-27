using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserTokenDto : BaseEntity
    {
        public string? Refresh { get; set; }
        public string? Email { get; set; }
        public string? IpAddress { get; set; }
        public string? Device { get; set; }

        public Guid UserId { get; set; }

        public UserToken Convert() => new()
        {
            Refresh = Refresh,
            Email = Email,
            IpAddress = IpAddress,
            Device = Device,

            UserId = UserId
        };
    }
}
