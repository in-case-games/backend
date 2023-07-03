using Review.DAL.Entities;
using System.Text.Json.Serialization;

namespace Review.BLL.Models
{
    public class UserReviewResponse : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
        public int Score { get; set; }
        public bool IsApproved { get; set; } = false;

        public Guid UserId { get; set; }

        public List<ReviewImageResponse>? Images { get; set; }
    }
}
