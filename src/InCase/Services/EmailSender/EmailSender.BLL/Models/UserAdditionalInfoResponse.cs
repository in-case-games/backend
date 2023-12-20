using EmailSender.DAL.Entities;

namespace EmailSender.BLL.Models
{
    public class UserAdditionalInfoResponse : BaseEntity
    {
        public bool IsNotifyEmail { get; set; }

        public Guid UserId { get; set; }
    }
}
