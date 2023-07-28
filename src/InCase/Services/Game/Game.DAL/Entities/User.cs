using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public UserAdditionalInfo? AdditionalInfo { get; set; }
        [JsonIgnore]
        public IEnumerable<UserPathBanner>? Paths { get; set; }
        [JsonIgnore]
        public IEnumerable<UserOpening>? Openings { get; set; }
        [JsonIgnore]
        public UserPromocode? Promocode { get; set; }
    }
}
