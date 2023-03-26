using Test.Domain.Entities;

namespace Test.Domain.Dtos
{
    public class LootBoxBannerDto : BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Image { get; set; } = "";

        public Guid BoxId { get; set; }

        public LootBoxBanner Convert() => new()
        {
            IsActive = IsActive,
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            Image = Image,
            BoxId = BoxId
        };
    }
}
