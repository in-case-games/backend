namespace CaseApplication.DomainLayer.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public int UserAge { get; set; }
        public decimal UserBalance { get; set; }
        public decimal UserAbleToPay { get; set; }
    }
}