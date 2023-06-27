using Game.BLL.Models;
using Game.DAL.Entities;

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
    }
}
