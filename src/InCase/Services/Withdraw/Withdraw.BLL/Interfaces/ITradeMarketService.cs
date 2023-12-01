using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface ITradeMarketService
    {
        public Task<BalanceMarketResponse> GetBalanceAsync(CancellationToken cancellation = default);
        public Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string trade, CancellationToken cancellation = default);
        public Task<ItemInfoResponse> GetItemInfoAsync(string idForMarket, string game, CancellationToken cancellation = default);
        public Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, CancellationToken cancellation = default);
    }
}
