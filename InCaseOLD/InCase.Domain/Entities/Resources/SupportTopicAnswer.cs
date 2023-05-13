using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
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

        public SupportTopicAnswerDto Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Content = Content,
            Date = Date,
            PlaintiffId = Plaintiff?.Id ?? PlaintiffId,
            TopicId = Topic?.Id ?? TopicId
        };
    }
}
