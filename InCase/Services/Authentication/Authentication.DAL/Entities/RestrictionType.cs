using System.Text.Json.Serialization;

namespace Authentication.DAL.Entities
{
    public class RestrictionType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserRestriction? Restriction { get; set; }
    }
}
