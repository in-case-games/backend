using Infrastructure.MassTransit.Resources;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Helpers
{
    public static class GameItemTransformer
    {
        public static GameItem ToEntity(this GameItemTemplate template) => new()
        {
            Id = template.Id,
            Cost = template.Cost,
            GameId = template.GameId,
            IdForMarket = template.IdForMarket
        };
    }
}
