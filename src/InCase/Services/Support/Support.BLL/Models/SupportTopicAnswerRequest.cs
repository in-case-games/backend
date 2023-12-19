using Support.DAL.Entities;

namespace Support.BLL.Models
{
    public class SupportTopicAnswerRequest : BaseEntity
    {
        public string? Content { get; set; }
        public DateTime Date { get; set; }

        public Guid? PlaintiffId { get; set; }
        public Guid TopicId { get; set; }
    }
}
