using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Resources
{
    public class UserRole : BaseEntity
    {
        public string? RoleName { get; set; }
        [JsonIgnore]
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
