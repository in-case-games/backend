namespace wdskills.DomainLayer.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        public User? User { get; set; }
        public int UserAge { get; set; }
        public decimal UserBalance { get; set; }
        public decimal UserEffectAbleToPay { get; set; }
        public UserRole? UserRole { get; set; }
    }
}