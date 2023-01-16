namespace wdskills.DomainLayer.Entities
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public ICollection<UserAdditionalInfo>? UserAdditionalInfos { get; set; }
        public ICollection<UserInventory>? UserInventories { get; set; }
        public ICollection<UserRestriction>?  UserRestrictions { get; set; }
    }
}
