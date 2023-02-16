using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities
{
    public class GameCase : BaseEntity
    {
        public string? GameCaseName { get; set; }
        public string? GroupCasesName { get; set; }
        public decimal GameCaseCost { get; set; }
        public decimal GameCaseBalance { get; set; } = 0M;
        public string? GameCaseImage { get; set; }
        public decimal RevenuePrecentage { get; set; } = 0.1M;
        public List<CaseInventory>? СaseInventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpeningCases>? UserHistoryOpeningCases { get; set; }
    }
}
