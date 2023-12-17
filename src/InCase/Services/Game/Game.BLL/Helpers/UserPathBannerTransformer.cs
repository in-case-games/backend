﻿using Game.BLL.Models;
using Game.DAL.Entities;

namespace Game.BLL.Helpers
{
    public static class UserPathBannerTransformer
    {
        public static UserPathBannerResponse ToResponse(this UserPathBanner banner) =>
            new()
            {
                Box = banner.Box,
                FixedCost = banner.FixedCost,
                Id = banner.Id,
                Item = banner.Item,
                NumberSteps = banner.NumberSteps,
            };

        public static List<UserPathBannerResponse> ToResponse(this List<UserPathBanner> banners)
        {
            List<UserPathBannerResponse> response = new();

            foreach (var banner in banners)
                response.Add(ToResponse(banner));

            return response;
        }
    }
}
