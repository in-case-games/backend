using Infrastructure.MassTransit.Resources;
using Resources.BLL.Entities;
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

        public static List<LootBoxBannerResponse> ToResponse(this List<LootBoxBanner> banners)
        {
            var response = new List<LootBoxBannerResponse>();

            foreach (var banner in banners) response.Add(ToResponse(banner));

            return response;
        }

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
