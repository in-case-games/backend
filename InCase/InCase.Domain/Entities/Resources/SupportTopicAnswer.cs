using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class SupportTopicAnswer : BaseEntity
    {
        public string? Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public Guid PlaintiffId { get; set; }
        [JsonIgnore]
        public Guid TopicId { get; set; }

        public User? Plaintiff { get; set; }

        [JsonIgnore]
        public SupportTopic? Topic { get; set; }
        [JsonIgnore]
        public List<AnswerImage>? AnswerImage { get; set; }

        public SupportTopicAnswerDto Convert() => new()
        {
            Content = Content,
            Date = Date,
            PlaintiffId = Plaintiff?.Id ?? PlaintiffId,
            TopicId = Topic?.Id ?? TopicId
        };
    }
}
