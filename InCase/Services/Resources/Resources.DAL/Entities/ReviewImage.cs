using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class ReviewImage : BaseEntity
    {
        public string? ImageUri { get; set; } = "";
        [JsonIgnore]
        public Guid ReviewId { get; set; }
        [JsonIgnore]
        public UserReview? Review { get; set; }
    }
}
