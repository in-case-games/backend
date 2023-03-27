using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Dtos
{
    public class PromocodesUsedByUserDto: BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid PromocodeId { get; set; }
    }
}
