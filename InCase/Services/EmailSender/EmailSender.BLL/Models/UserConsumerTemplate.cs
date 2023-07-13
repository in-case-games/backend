using EmailSender.DAL.Entities;

namespace EmailSender.BLL.Models
{
    public class UserConsumerTemplate : BaseEntity
    {
        public string? Email { get; set; }
        public string? Login { get; set; }
    }
}
