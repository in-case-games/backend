using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class SupportTopicDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; } = false;

        public Guid UserId { get; set; }
        public Guid? SupportId { get; set; }

        public SupportTopic Convert() => new()
        {
            Title = Title,
            Content = Content,
            Date = Date,
            IsClosed = IsClosed,
            UserId = UserId,
            SupportId = SupportId
        };
    }
}
