using Infrastructure.MassTransit.Resources;
using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
    public static class GameItemTransformer
    {
        public static GameItemResponse ToResponse(this GameItem item) =>
            new() { 
                Id = item.Id,
                Name = item.Name,
                Cost = item.Cost,
                Quality = item.Quality?.Name,
                Rarity = item.Rarity?.Name,
                Type = item.Type?.Name,
                Game = item.Game?.Name,
            };

        public static List<GameItemResponse> ToResponse(this IEnumerable<GameItem> items)
        {
            List<GameItemResponse> result = new();

            foreach(var item in items)
                result.Add(ToResponse(item));

            return result;
        }

        public static GameItem ToEntity(this GameItemRequest request, bool isNewGuid = false) =>
            new() { 
                Id = isNewGuid ? Guid.NewGuid() : request.Id,
                Cost = request.Cost,
                GameId = request.GameId,
                HashName = request.HashName,
                QualityId = request.QualityId,
                RarityId = request.RarityId,
                Name = request.Name,
                TypeId = request.TypeId
            };

        public static GameItemTemplate ToTemplate(this GameItem entity, string? idForMarket, bool isDeleted = false) => new()
        {
            Id = entity.Id,
            Cost = entity.Cost,
            GameId = entity.GameId,
            HashName = entity.HashName,
            IsDeleted = isDeleted,
            Name = entity.Name,
            QualityId = entity.QualityId,
            RarityId = entity.RarityId,
            TypeId = entity.TypeId,
            IdForMarket = idForMarket
        };
    }
}
