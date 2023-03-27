using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class GameItemQuality : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public LootBoxInventory? Inventory { get; set; }
    }
}
