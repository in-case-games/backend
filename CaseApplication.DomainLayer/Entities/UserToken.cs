using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class UserToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenCreationTime { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? UserIpAddress { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
