using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class SupportTopicAnswerDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }

        public Guid PlaintiffId { get; set; }
        public Guid TopicId { get; set; }

        public SupportTopicAnswer Convert() => new()
        {
            Title = Title,
            Content = Content,
            Image = Image,
            Date = Date,
            PlaintiffId = PlaintiffId,
            TopicId = TopicId
        };
    }
}
