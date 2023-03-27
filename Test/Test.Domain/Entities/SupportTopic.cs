using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
{
    public class SupportTopic : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; } = false;

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid? SupportId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public User? Support { get; set; }

        public List<SupportTopicAnswer>? Answers { get; set; }

        public SupportTopicDto Convert() => new()
        {
            Title = Title,
            Content = Content,
            Date = Date,
            IsClosed = IsClosed,
            UserId = User?.Id ?? UserId,
            SupportId = Support?.Id ?? SupportId
        };
    }
}
