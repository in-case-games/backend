using InCase.Domain.Entities;

namespace InCase.Domain.Dtos
{
    public class UserReviewDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public bool IsApproved { get; set; } = false;

        public Guid UserId { get; set; }

        public UserReview Convert() => new()
        {
            Title = Title,
            Content = Content,
            Image = Image,
            IsApproved = IsApproved,

            UserId = UserId
        };
    }
}
