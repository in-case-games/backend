using EmailSender.DAL.Entities;

namespace EmailSender.BLL.Models
{
    public class UserRequest : BaseEntity
    {
        public string? Email { get; set; }
    }
}
