using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class PromocodesUsedByUser: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PromocodeId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Promocode? Promocode { get; set; }
    }
}