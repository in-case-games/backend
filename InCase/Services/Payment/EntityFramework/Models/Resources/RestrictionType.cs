using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class RestrictionType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserRestriction? Restriction { get; set; }
    }
}
