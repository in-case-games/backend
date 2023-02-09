using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class PromocodesUsedByUser: BaseEntity
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid PromocodeId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public Promocode? Promocode { get; set; }
    }
}