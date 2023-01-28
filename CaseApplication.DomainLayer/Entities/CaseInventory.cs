using System.Text.Json.Serialization;

namespace CaseApplication.DomainLayer.Entities
{
    public class CaseInventory : BaseEntity
    {
        public Guid GameCaseId { get; set; }
        public Guid GameItemId { get; set; }

        [JsonIgnore]
        public GameCase? GameCase { get; set; }
        [JsonIgnore]
        public GameItem? GameItem { get; set; }

        public int NumberItemsCase { get; set; }
        public int LossChance { get; set; }
    }
}
