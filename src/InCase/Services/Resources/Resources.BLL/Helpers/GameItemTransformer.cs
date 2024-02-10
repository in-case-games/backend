using Infrastructure.MassTransit.Resources;
using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers;
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
            HashName = item.HashName,
            IdForMarket = item.IdForMarket,
            UpdateDate = item.UpdateDate,
        };

    public static List<GameItemResponse> ToResponse(this IEnumerable<GameItem> items) =>
        items.Select(ToResponse).ToList();

    public static GameItem ToEntity(this GameItemRequest request, bool isNewGuid = false) =>
        new() { 
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Cost = request.Cost,
            GameId = request.GameId,
            HashName = request.HashName,
            QualityId = request.QualityId,
            RarityId = request.RarityId,
            Name = request.Name,
            TypeId = request.TypeId,
            IdForMarket = request.IdForMarket
        };

    public static GameItemTemplate ToTemplate(this GameItem entity, bool isDeleted = false) => 
        new()
        {
            Id = entity.Id,
            Cost = entity.Cost,
            GameName = entity.Game?.Name,
            HashName = entity.HashName,
            IsDeleted = isDeleted,
            Name = entity.Name,
            IdForMarket = entity.IdForMarket
        };
}