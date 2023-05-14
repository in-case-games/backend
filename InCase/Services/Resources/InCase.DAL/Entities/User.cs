using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class User : BaseEntity
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public UserAdditionalInfo? AdditionalInfo { get; set; }

        public IEnumerable<SupportTopic>? Topics { get; set; }
        public IEnumerable<UserRestriction>? Restrictions { get; set; }
        public IEnumerable<UserRestriction>? OwnerRestrictions { get; set; }
        public IEnumerable<UserReview>? Reviews { get; set; }
        public IEnumerable<UserHistoryPayment>? HistoryPayments { get; set; }

        [JsonIgnore]
        public IEnumerable<SupportTopicAnswer>? Answers { get; set; }
        [JsonIgnore]
        public IEnumerable<UserHistoryPromocode>? HistoryPromocodes { get; set; }
        [JsonIgnore]
        public IEnumerable<UserPathBanner>? Paths { get; set; }
        [JsonIgnore]
        public IEnumerable<UserHistoryOpening>? HistoryOpenings { get; set; }
        [JsonIgnore]
        public IEnumerable<UserInventory>? Inventories { get; set; }
        [JsonIgnore]
        public IEnumerable<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
    }
}
