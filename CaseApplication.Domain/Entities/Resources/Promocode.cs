using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities.Resources
{
    public class Promocode : BaseEntity
    {
        [JsonIgnore]
        public Guid PromocodeTypeId { get; set; }
        public string? PromocodeName { get; set; }
        public decimal PromocodeDiscount { get; set; }
        public int PromocodeUsesCount { get; set; }
        public DateTime? PromocodeExpiryTime { get; set; }
        public PromocodeType? PromocodeType { get; set; }
        [JsonIgnore]
        public List<PromocodesUsedByUser>? PromocodesUsedByUsers { get; set; }
    }
}
