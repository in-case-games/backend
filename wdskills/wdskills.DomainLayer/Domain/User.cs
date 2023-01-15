namespace wdskills.DomainLayer.Domain
{
    public class User
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public decimal UserBalance { get; set; }
        public UserRole? UserRole { get; set; }
        public UserInventory? UserInventory { get; set; }
        public UserEffect? UserLimitationsEffects { get; set; }
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
