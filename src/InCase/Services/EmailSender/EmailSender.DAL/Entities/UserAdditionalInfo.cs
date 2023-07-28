using System.Text.Json.Serialization;

namespace EmailSender.DAL.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public bool IsNotifyEmail { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
