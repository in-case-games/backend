using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class Promocode: BaseEntity
    {
        public Guid PromocodeTypeId { get; set; }
        public string? PromocodeName { get; set; }
        public decimal PromocodeDiscount { get; set; }
        public int PromocodeUsesCount { get; set; }
        [JsonIgnore]
        public PromocodeType? PromocodeType { get; set; }
        [JsonIgnore]
        public List<PromocodesUsedByUser>? PromocodesUsedByUsers { get; set; }
    }
}
