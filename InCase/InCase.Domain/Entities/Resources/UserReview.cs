using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class UserReview : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public bool IsApproved { get; set; } = false;

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public UserReview Convert() => new()
        {
            Title = Title,
            Content = Content,
            Image = Image,
            IsApproved = IsApproved,

            UserId = User?.Id ?? UserId
        };
    }
}
