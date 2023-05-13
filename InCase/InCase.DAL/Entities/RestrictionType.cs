using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class RestrictionType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserRestriction? Restriction { get; set; }
    }
}
