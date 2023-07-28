using Support.DAL.Entities;

namespace Support.BLL.Models
{
    public class AnswerImageRequest : BaseEntity
    {
        public Guid AnswerId { get; set; }
    }
}
