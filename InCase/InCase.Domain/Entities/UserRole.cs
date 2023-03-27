using System.Text.Json.Serialization;

namespace InCase.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
