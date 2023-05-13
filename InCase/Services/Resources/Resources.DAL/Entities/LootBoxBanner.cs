using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class LootBoxBanner : BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? ImageUri { get; set; } = "";

        [JsonIgnore]
        public Guid BoxId { get; set; }

        public LootBox? Box { get; set; }

        [JsonIgnore]
        public List<UserPathBanner>? Paths { get; set; }
    }
}
