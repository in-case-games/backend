using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class GameCase : BaseEntity
    {
        public string? GameCaseName { get; set; }
        public decimal GameCaseCost { get; set; }
        public string? GameCaseImage { get; set; }
        public decimal RevenuePrecentage { get; set; } = 0.1M;

        [JsonIgnore]
        public List<CaseInventory>? СaseInventories { get; set; }
    }
}
