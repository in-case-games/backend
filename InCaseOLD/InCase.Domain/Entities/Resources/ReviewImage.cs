using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class ReviewImage : BaseEntity
    {
        public string? ImageUri { get; set; } = "";
        [JsonIgnore]
        public Guid ReviewId { get; set; }
        [JsonIgnore]
        public UserReview? Review { get; set; }

        public ReviewImageDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            ImageUri = ImageUri,
            ReviewId = Review?.Id ?? ReviewId,
        };
    }
}
