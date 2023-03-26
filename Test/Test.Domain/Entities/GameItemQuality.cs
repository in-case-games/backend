using System.Text.Json.Serialization;

namespace Test.Domain.Entities
{
    public class GameItemQuality : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public LootBoxInventory? Inventory { get; set; }
    }
}
