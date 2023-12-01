using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IWithdrawItemService
    {
        public Task<BalanceMarketResponse> GetBalanceAsync(string marketName, CancellationToken cancellation = default);
        public Task<ItemInfoResponse> GetItemInfoAsync(GameItem item, CancellationToken cancellation = default);
        public Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl, CancellationToken cancellation = default);
        public Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, CancellationToken cancellation = default);
    }
}
