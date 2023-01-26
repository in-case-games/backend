using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid UserRoleId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public UserRole? UserRole { get; set; }

        public int UserAge { get; set; }
        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
    }
}