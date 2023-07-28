using EmailSender.DAL.Entities;
using System.Text.Json.Serialization;

namespace EmailSender.BLL.Models
{
    public class UserAdditionalInfoResponse : BaseEntity
    {
        public bool IsNotifyEmail { get; set; }

        public Guid UserId { get; set; }
    }
}
