using System.Text.Json.Serialization;
using Test.Domain.Dtos;

namespace Test.Domain.Entities
{
    public class LootBoxBanner : BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Image { get; set; } = "";

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
            Image = Image,
            BoxId = Box?.Id ?? BoxId,
        };
    }
}
