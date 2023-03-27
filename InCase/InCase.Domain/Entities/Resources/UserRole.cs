using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class UserRole : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
