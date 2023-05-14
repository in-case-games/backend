using System.Text.Json.Serialization;

namespace SupportTopic.DAL.Entities
{
    public class SupportTopic : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsClosed { get; set; } = false;

        public IEnumerable<SupportTopicAnswer>? Answers { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
