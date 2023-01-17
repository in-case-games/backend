namespace CaseApplication.DomainLayer.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public int UserAge { get; set; }
        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
        public Guid UserRoleId { get; set; }
        public UserRole? UserRole { get; set; }
    }
}