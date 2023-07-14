using EmailSender.DAL.Entities;

namespace EmailSender.BLL.Models
{
    public class UserResponse : BaseEntity
    {
        public string? Email { get; set; }
    }
}
