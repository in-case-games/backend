using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class CaseInventory : BaseEntity
    {
        [JsonIgnore]
        public Guid GameCaseId { get; set; }
        [JsonIgnore]
        public Guid GameItemId { get; set; }
        public int NumberItemsCase { get; set; }
        public int LossChance { get; set; }
        public GameCase? GameCase { get; set; }
        public GameItem? GameItem { get; set; }
        
    }
}
