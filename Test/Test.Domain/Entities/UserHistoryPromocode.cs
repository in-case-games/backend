using System.Text.Json.Serialization;

namespace Test.Domain.Entities
{
    public class UserHistoryPromocode : BaseEntity
    { 
        public DateTime? Date { get; set; }
        public bool IsActivated { get; set; } = false;
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid PromocodeId { get; set; }

        public User? User { get; set; }
        public Promocode? Promocode { get; set; }

        public UserHistoryPromocode Convert() => new()
        {
            Date = Date,
            IsActivated = IsActivated,
            UserId = User?.Id ?? UserId,
            PromocodeId = Promocode?.Id ?? PromocodeId
        };
    }
}
