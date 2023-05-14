using System.Text.Json.Serialization;

namespace Game.DAL.Entities
{
    public class LootBoxBanner : BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public LootBox? Box { get; set; }

        [JsonIgnore]
        public Guid BoxId { get; set; }

        [JsonIgnore]
        public IEnumerable<UserPathBanner>? Paths { get; set; }
    }
}
