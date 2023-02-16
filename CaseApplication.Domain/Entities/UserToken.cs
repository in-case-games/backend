using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public string? EmailToken { get; set; }
        public DateTime RefreshTokenCreationTime { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? UserIpAddress { get; set; }
        public string? UserPlatfrom { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
