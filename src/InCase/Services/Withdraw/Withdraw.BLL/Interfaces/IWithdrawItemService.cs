using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IWithdrawItemService
    {
        public Task<BalanceMarketResponse> GetBalanceAsync(string marketName);
        public Task<ItemInfoResponse> GetItemInfoAsync(GameItem item);
        public Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl);
        public Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history);
    }
}
