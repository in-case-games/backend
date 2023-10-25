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

        public static GameItem ToEntity(this GameItemTemplate template) => new()
        {
            Id = template.Id,
            Cost = template.Cost
        };
    }
}
