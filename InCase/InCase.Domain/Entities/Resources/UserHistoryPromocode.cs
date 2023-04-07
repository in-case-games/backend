using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class UserHistoryPromocode : BaseEntity
    {
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public bool IsActivated { get; set; } = false;
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid PromocodeId { get; set; }

        public User? User { get; set; }
        public Promocode? Promocode { get; set; }

        public UserHistoryPromocode Convert() => new()
        {
            Id = Id,
            Date = Date,
            IsActivated = IsActivated,
            UserId = User?.Id ?? UserId,
            PromocodeId = Promocode?.Id ?? PromocodeId
        };
    }
}
