using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities.Resources
{
    public class LootBoxBanner : BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Uri { get; set; } = "";

        [JsonIgnore]
        public Guid BoxId { get; set; }

        public LootBox? Box { get; set; }

        [JsonIgnore]
        public List<UserPathBanner>? Paths { get; set; }

        public LootBoxBannerDto Convert() => new()
        {
            IsActive = IsActive,
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            Uri = Uri,
            BoxId = Box?.Id ?? BoxId,
        };
    }
}
