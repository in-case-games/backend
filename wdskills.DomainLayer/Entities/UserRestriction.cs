namespace wdskills.DomainLayer.Entities
{
    public class UserRestriction : BaseEntity
    {
        public User? User { get; set; }
        public string? RestrictionName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}