namespace Identity.DAL.Entities
{
    public class User : BaseEntity
    {
        public string? Email { get; set; }
        public string? Login { get; set; }

        public IEnumerable<UserRestriction>? Restrictions { get; set; }
        public UserAdditionalInfo? AdditionalInfo { get; set; }
    }
}
