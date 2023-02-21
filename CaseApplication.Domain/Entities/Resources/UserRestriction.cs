using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Resources
{
    public class UserRestriction : BaseEntity
    {
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public string? RestrictionName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}