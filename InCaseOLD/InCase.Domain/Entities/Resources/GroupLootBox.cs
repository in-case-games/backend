using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class GroupLootBox : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public LootBoxGroup? Group { get; set; }
    }
}
