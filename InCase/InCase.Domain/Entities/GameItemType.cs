using System.Text.Json.Serialization;

namespace InCase.Domain.Entities
{
    public class GameItemType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public GameItem? Item { get; set; }
    }
}
