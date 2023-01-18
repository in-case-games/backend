using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        [JsonIgnore]
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
