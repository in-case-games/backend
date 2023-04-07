using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
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

        public SupportTopicDto Convert() => new()
        {
            Id = Id,
            Title = Title,
            Content = Content,
            Date = Date,
            IsClosed = IsClosed,
            UserId = User?.Id ?? UserId
        };
    }
}
