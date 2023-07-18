using Game.BLL.Models;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Helpers
{
    public static class GameItemTransformer
    {
        public static GameItemResponse ToResponse(this GameItem item) => new()
        {
            Id = item.Id,
            Cost = item.Cost,
        };

        public static List<GameItemResponse> ToResponse(this List<GameItem> items)
        {
            List<GameItemResponse> response = new();

            foreach(var item in items)
                response.Add(ToResponse(item));

            return response;
        }

        public static GameItem ToEntity(this GameItemRequest request, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Cost = request.Cost
        };

        public static GameItemRequest ToRequest(this GameItemTemplate template) => new()
        {
            Id = template.Id,
            Cost = template.Cost,
        };
    }
}
