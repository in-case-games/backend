using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface ITradeMarketService
    {
        public Task<decimal> GetBalanceAsync();
        public Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string trade);
        public Task<ItemInfoResponse> GetItemInfoAsync(string idForMarket, string game);
        public Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history);
    }
}
