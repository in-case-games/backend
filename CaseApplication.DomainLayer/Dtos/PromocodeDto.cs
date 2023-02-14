using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Dtos
{
    public class PromocodeDto: BaseEntity
    {
        public Guid PromocodeTypeId { get; set; }
        public string? PromocodeName { get; set; }
        public decimal PromocodeDiscount { get; set; }
        public int PromocodeUsesCount { get; set; }
    }
}
