using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class GameItemType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public GameItem? Item { get; set; }
    }
}
