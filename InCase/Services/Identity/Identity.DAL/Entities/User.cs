namespace Identity.DAL.Entities
{
    public class User : BaseEntity
    {
        public IEnumerable<UserRestriction>? Restrictions { get; set; }
        public UserAdditionalInfo? AdditionalInfo { get; set; }
    }
}
