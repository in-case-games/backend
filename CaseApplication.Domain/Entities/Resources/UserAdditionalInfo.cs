using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Resources
{
    public class UserAdditionalInfo : BaseEntity
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid UserRoleId { get; set; }

        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
        public bool IsConfirmedAccount { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public UserRole? UserRole { get; set; }
    }
}