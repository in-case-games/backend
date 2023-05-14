using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
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

        public UserHistoryPromocode Convert(bool isNewGuid = true) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : Id,
            Date = Date,
            IsActivated = IsActivated,
            UserId = User?.Id ?? UserId,
            PromocodeId = Promocode?.Id ?? PromocodeId
        };
    }
}
