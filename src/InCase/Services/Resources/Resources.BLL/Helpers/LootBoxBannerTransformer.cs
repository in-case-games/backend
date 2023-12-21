using Infrastructure.MassTransit.Resources;
using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
    public static class LootBoxBannerTransformer
    {
        public static LootBoxBannerResponse ToResponse(this LootBoxBanner banner) =>
            new()
            {
                Id = banner.Id,
                Box = banner?.Box?.ToResponse(),
                CreationDate = banner!.CreationDate,
                ExpirationDate = banner?.ExpirationDate,
            };

        public static List<LootBoxBannerResponse> ToResponse(this List<LootBoxBanner> banners) =>
            banners.Select(ToResponse).ToList();

        public static LootBoxBannerTemplate ToTemplate(this LootBoxBanner entity, bool isDeleted = false) => new()
        {
            Id = entity.Id,
            BoxId = entity.BoxId,
            CreationDate = entity.CreationDate,
            ExpirationDate = entity.ExpirationDate,
            IsDeleted = isDeleted
        };
    }
}
