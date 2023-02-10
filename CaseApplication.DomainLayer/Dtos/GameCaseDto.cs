using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Dtos
{
    public class GameCaseDto : BaseEntity
    {
        public string? GameCaseName { get; set; }
        public string? GroupCasesName { get; set; }
        public decimal GameCaseCost { get; set; }
        public decimal GameCaseBalance { get; set; } = 0M;
        public string? GameCaseImage { get; set; }
        public decimal RevenuePrecentage { get; set; } = 0.1M;
    }
}
