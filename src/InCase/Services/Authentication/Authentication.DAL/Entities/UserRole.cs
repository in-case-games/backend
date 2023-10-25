using System.Text.Json.Serialization;

namespace Authentication.DAL.Entities
{
    public class UserRole : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserAdditionalInfo? AdditionalInfo { get; set; }
    }
}
