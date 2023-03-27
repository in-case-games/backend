using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class ReviewImage : BaseEntity
    {
        public string? Uri { get; set; } = "";
        [JsonIgnore]
        public Guid ReviewId { get; set; }

        public UserReview? Review { get; set; }

        public ReviewImageDto Convert() => new()
        {
            Uri = Uri,
            ReviewId = Review?.Id ?? ReviewId,
        };
    }
}
