using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class LootBoxBannerDto : BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? ImageUri { get; set; } = "";

        public Guid BoxId { get; set; }

        public LootBoxBanner Convert() => new()
        {
            IsActive = IsActive,
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            ImageUri = ImageUri,
            BoxId = BoxId
        };
    }
}
