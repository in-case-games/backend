using CaseApplication.Domain.Entities;
using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Dtos
{
    public class PromocodesUsedByUserDto: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PromocodeId { get; set; }
    }
}
