using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Internal
{
    public class UserHistoryOpeningCases : BaseEntity
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid GameItemId { get; set; }
        [JsonIgnore]
        public Guid GameCaseId { get; set; }
        public DateTime? CaseOpenAt { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public GameItem? GameItem { get; set; }
        public GameCase? GameCase { get; set; }
    }
}
