using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class GameItem : BaseEntity
    {
        public string? GameItemName { get; set; }
        public decimal GameItemCost { get; set; }
        public string? GameItemImage { get; set; }
        public string? GameItemRarity { get; set; }

        [JsonIgnore]
        public List<UserInventory>? UserInventories { get; set; }
        [JsonIgnore]
        public List<CaseInventory>? CaseInventories { get; set; }
        
    }
}
