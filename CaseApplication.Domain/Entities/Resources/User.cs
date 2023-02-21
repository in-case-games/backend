using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Resources
{
    public class User : BaseEntity
    {
        public string? UserLogin { get; set; }
        public string? UserImage { get; set; }
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
        public List<UserRestriction>? UserRestrictions { get; set; }
        public List<UserInventory>? UserInventories { get; set; }
        public List<UserHistoryOpeningCases>? UserHistoryOpeningCases { get; set; }
        public List<PromocodesUsedByUser>? PromocodesUsedByUsers { get; set; }
        [JsonIgnore]
        public List<UserToken>? UserTokens { get; set; }
    }
}
