using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class LootBoxBanner : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public LootBox? Box { get; set; }

        [JsonIgnore]
        public Guid BoxId { get; set; }
    }
}
