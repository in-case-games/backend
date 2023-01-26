using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class UserRole : BaseEntity
    {
        public string? RoleName { get; set; }

        [JsonIgnore]
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
