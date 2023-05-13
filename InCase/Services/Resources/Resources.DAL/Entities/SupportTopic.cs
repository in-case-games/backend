using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class SupportTopic : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsClosed { get; set; } = false;

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public List<SupportTopicAnswer>? Answers { get; set; }
    }
}
