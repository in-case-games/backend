using InCase.Domain.Dtos;
using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class UserReview : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsApproved { get; set; } = false;

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public List<ReviewImage>? Images { get; set; }

        public UserReviewDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Title = Title,
            Content = Content,
            CreationDate = CreationDate,
            IsApproved = IsApproved,
            UserId = User?.Id ?? UserId
        };
    }
}
