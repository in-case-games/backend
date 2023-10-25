using System.Text.Json.Serialization;

namespace Review.DAL.Entities
{
    public class UserReview : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
        public int Score { get; set; }
        public bool IsApproved { get; set; } = false;

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public IEnumerable<ReviewImage>? Images { get; set; }
    }
}
