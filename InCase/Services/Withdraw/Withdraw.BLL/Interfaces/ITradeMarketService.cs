using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface ITradeMarketService
    {
        public Task<decimal> GetBalance();
        public Task<BuyItemResponse> BuyItem(ItemInfoResponse info, string trade);
        public Task<ItemInfoResponse> GetItemInfo(string idForMarket, string game);
        public Task<TradeInfoResponse> GetTradeInfo(UserHistoryWithdraw history);
    }
}
