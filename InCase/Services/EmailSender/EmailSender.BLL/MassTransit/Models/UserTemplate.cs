using EmailSender.DAL.Entities;

namespace EmailSender.BLL.MassTransit.Models
{
    public class UserTemplate : BaseEntity
    {
        public string? Email { get; set; }
        public string? Login { get; set; }
        public bool IsDeleted { get; set; }
    }
}
