using System.Text.Json.Serialization;

namespace Identity.DAL.Entities
{
    public class User : BaseEntity
    {
        public string? Login { get; set; }

        [JsonIgnore]
        public IEnumerable<UserRestriction>? Restrictions { get; set; }
        [JsonIgnore]
        public UserAdditionalInfo? AdditionalInfo { get; set; }
    }
}
