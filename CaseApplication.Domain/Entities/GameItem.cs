using System.Text.Json.Serialization;

namespace CaseApplication.Domain.Entities
{
    public class GameItem : BaseEntity
    {
        public string? GameItemName { get; set; }
        public decimal GameItemCost { get; set; }
        public string? GameItemImage { get; set; }
        public string? GameItemRarity { get; set; }
        public string? GameItemType { get; set; }

        [JsonIgnore]
        public List<UserInventory>? UserInventories { get; set; }
        [JsonIgnore]
        public List<CaseInventory>? CaseInventories { get; set; }
        [JsonIgnore]
        public List<UserHistoryOpeningCases>? UserHistoryOpeningCases { get; set; }
    }
}
