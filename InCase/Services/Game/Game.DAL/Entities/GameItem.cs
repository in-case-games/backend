using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class GameItem : BaseEntity
    {
        public decimal Cost { get; set; }

        [JsonIgnore]
        public IEnumerable<LootBoxInventory>? Inventories { get; set; }
        [JsonIgnore]
        public IEnumerable<UserPathBanner>? PathBanners { get; set; }
    }
}
