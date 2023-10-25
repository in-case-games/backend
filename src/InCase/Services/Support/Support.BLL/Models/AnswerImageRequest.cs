using Support.DAL.Entities;

namespace Support.BLL.Models
{
    public class AnswerImageRequest : BaseEntity
    {
        public string? Image { get; set; }
        public Guid AnswerId { get; set; }
    }
}
