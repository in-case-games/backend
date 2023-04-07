using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class SupportTopicAnswerDto : BaseEntity
    {
        public string? Content { get; set; }
        public DateTime Date { get; set; }

        public Guid? PlaintiffId { get; set; }
        public Guid TopicId { get; set; }

        public SupportTopicAnswer Convert() => new()
        {
            Id = Id,
            Content = Content,
            Date = Date,
            PlaintiffId = PlaintiffId,
            TopicId = TopicId
        };
    }
}
