using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class UserToken : BaseEntity
    {
        public string? Refresh { get; set; }
        public string? Email { get; set; }
        public string? IpAddress { get; set; }
        public string? Device { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public UserTokenDto Convert() => new()
        {
            Refresh = Refresh,
            Email = Email,
            IpAddress = IpAddress,
            Device = Device,

            UserId = User?.Id ?? UserId
        };
    }
}
