using System.Text.Json.Serialization;

namespace Support.DAL.Entities
{
    public class SupportTopic : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; }

        public IEnumerable<SupportTopicAnswer>? Answers { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
