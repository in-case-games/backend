using Infrastructure.MassTransit.Resources;
using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
    public static class LootBoxTransformer
    {
        public static LootBoxResponse ToResponse(this LootBox box) =>
            new()
            {
                Id = box.Id,
                Cost = box.Cost,
                Name = box.Name,
                IsLocked = box.IsLocked,
                Game = box.Game?.Name
            };

        public static List<LootBoxResponse> ToResponse(this IEnumerable<LootBox> boxes)
        { 
            var response = new List<LootBoxResponse>();

            foreach (var box in boxes) response.Add(ToResponse(box));

            return response;
        }

        public static LootBoxTemplate ToTemplate(this LootBox entity, bool isDeleted = false) => new()
        {
            Id = entity.Id,
            Cost = entity.Cost,
            GameId = entity.GameId,
            IsDeleted = isDeleted,
            IsLocked = entity.IsLocked,
            Name = entity.Name
        };
    }
}
