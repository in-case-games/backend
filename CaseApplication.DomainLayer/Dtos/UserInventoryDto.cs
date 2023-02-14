using CaseApplication.DomainLayer.Entities;
using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Dtos
{
    public class UserInventoryDto: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GameItemId { get; set; }
    }
}
