using System.Text.Json.Serialization;

namespace Review.DAL.Entities;

public class ReviewImage : BaseEntity
{
    [JsonIgnore]
    public Guid ReviewId { get; set; }
    [JsonIgnore]
    public UserReview? Review { get; set; }
}