using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class UserHistoryOpeningCases : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GameItemId { get; set; }
        public Guid GameCaseId { get; set; }
        public DateTime? CaseOpenAt { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public GameItem? GameItem { get; set; }
        [JsonIgnore]
        public GameCase? GameCase { get; set; }
    }
}
