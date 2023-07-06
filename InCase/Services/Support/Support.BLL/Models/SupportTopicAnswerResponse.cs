using Support.DAL.Entities;

namespace Support.BLL.Models
{
    public class SupportTopicAnswerResponse : BaseEntity
    {
        public string? Content { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<AnswerImage>? Images { get; set; }

        public Guid? PlaintiffId { get; set; }
        public Guid TopicId { get; set; }
    }
}
