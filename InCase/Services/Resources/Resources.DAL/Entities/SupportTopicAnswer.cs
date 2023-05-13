using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class SupportTopicAnswer : BaseEntity
    {
        public string? Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public Guid? PlaintiffId { get; set; }
        [JsonIgnore]
        public Guid TopicId { get; set; }

        public User? Plaintiff { get; set; }

        [JsonIgnore]
        public SupportTopic? Topic { get; set; }
        public List<AnswerImage>? Images { get; set; }
    }
}
