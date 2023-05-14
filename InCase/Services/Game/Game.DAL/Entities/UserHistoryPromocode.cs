using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class UserHistoryPromocode : BaseEntity
    {
        public DateTime? Date { get; set; }
        public bool IsActivated { get; set; } = false;

        public User? User { get; set; }
        public Promocode? Promocode { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid PromocodeId { get; set; }
    }
}
