using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Dtos
{
    public class UserHistoryOpeningCasesDto: BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid GameItemId { get; set; }
        [Required]
        public Guid GameCaseId { get; set; }
        [Required]
        public DateTime? CaseOpenAt { get; set; }
    }
}
