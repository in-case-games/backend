using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserReviewDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; } = false;

        public Guid UserId { get; set; }

        public UserReview Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Title = Title,
            Content = Content,
            IsApproved = IsApproved,
            UserId = UserId
        };
    }
}
