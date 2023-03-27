using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities
{
    public class SupportTopicAnswer : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Guid PlaintiffId { get; set; }
        [JsonIgnore]
        public Guid TopicId { get; set; }

        public User? Plaintiff { get; set; }

        [JsonIgnore]
        public SupportTopic? Topic { get; set; }

        public SupportTopicAnswerDto Convert() => new()
        {
            Title = Title,
            Content = Content,
            Image = Image,
            Date = Date,
            PlaintiffId = Plaintiff?.Id ?? PlaintiffId,
            TopicId = Topic?.Id ?? TopicId
        };
    }
}
