using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class SupportTopicDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; } = false;

        public Guid UserId { get; set; }

        public SupportTopic Convert() => new()
        {
            Id = Id,
            Title = Title,
            Content = Content,
            Date = Date,
            IsClosed = IsClosed,
            UserId = UserId
        };
    }
}
