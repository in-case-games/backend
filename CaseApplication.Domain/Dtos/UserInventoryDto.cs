using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Dtos
{
    public class UserInventoryDto: BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid GameItemId { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }
}
