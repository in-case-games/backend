using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Dtos
{
    public class GameCaseDto : BaseEntity
    {
        [Required]
        public string? GameCaseName { get; set; }
        [Required]
        public string? GroupCasesName { get; set; }
        [Required]
        public decimal GameCaseCost { get; set; }
        public decimal GameCaseBalance { get; set; } = 0M;
        [Required]
        public string? GameCaseImage { get; set; }
        public decimal RevenuePrecentage { get; set; } = 0.1M;
    }
}
