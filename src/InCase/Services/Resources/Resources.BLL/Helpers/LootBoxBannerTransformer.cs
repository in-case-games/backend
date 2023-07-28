using Infrastructure.MassTransit.Resources;
using Resources.BLL.Entities;
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

        public static List<LootBoxBannerResponse> ToResponse(this List<LootBoxBanner> banners)
        {
            List<LootBoxBannerResponse> response = new();

            foreach (var banner in banners)
                response.Add(ToResponse(banner));

            return response;
        }

        public static LootBoxBanner ToEntity(
            this LootBoxBannerRequest request,
            bool isNewGuid = false,
            DateTime? creationDate = null) =>
            new()
            {
                Id = isNewGuid ? Guid.NewGuid() : request.Id,
                CreationDate = creationDate ?? DateTime.UtcNow,
                ExpirationDate = request.ExpirationDate,
                BoxId = request.BoxId,
            };

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
