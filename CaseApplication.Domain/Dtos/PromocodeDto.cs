using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Dtos
{
    public class PromocodeDto: BaseEntity
    {
        [Required]
        public Guid PromocodeTypeId { get; set; }
        [Required]
        public string? PromocodeName { get; set; }
        public decimal PromocodeDiscount { get; set; }
        public int PromocodeUsesCount { get; set; }
    }
}
