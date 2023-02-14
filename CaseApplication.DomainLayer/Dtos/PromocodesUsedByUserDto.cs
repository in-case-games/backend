using CaseApplication.DomainLayer.Entities;
using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Dtos
{
    public class PromocodesUsedByUserDto: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PromocodeId { get; set; }
    }
}
