using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Helpers
{
    public static class GameMarketTransformer
    {
        public static GameMarketResponse ToResponse(this GameMarket market) =>
            new()
            {
                Id = market.Id,
                Game = market.Game,
                Name = market.Name,
            };

        public static List<GameMarketResponse> ToResponse(this List<GameMarket> markets)
        {
            List<GameMarketResponse> response = new();

            foreach (var market in markets)
                response.Add(ToResponse(market));
            
            return response;
        }
    }
}
