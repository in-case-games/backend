using Support.DAL.Entities;

namespace Support.BLL.Models
{
    public class SupportTopicRequest : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; }

        public Guid UserId { get; set; }
    }
}
