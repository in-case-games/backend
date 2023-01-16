namespace wdskills.DomainLayer.Entities
{
    public class UserRole : BaseEntity
    {
        public string? RoleName { get; set; }
        public ICollection<UserAdditionalInfo>? UserAdditionalInfos { get; set; }
    }
}
