using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public string? RefreshToken { get; set; }
        public string? EmailToken { get; set; }
        public string? IpAddress { get; set; }
        public string? Device { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public UserTokenDto Convert() => new()
        {
            RefreshToken = RefreshToken,
            EmailToken = EmailToken,
            IpAddress = IpAddress,
            Device = Device,

            UserId = User?.Id ?? UserId
        };
    }
}
