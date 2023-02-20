using CaseApplication.Domain.Entities;
using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Dtos
{
    public class UserInventoryDto: BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GameItemId { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }
}
