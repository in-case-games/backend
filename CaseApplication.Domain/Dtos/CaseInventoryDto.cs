using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Dtos
{
    public class CaseInventoryDto: BaseEntity
    {
        [Required]
        public Guid GameCaseId { get; set; }
        [Required]
        public Guid GameItemId { get; set; }
        public int NumberItemsCase { get; set; }
        public int LossChance { get; set; }
    }
}
