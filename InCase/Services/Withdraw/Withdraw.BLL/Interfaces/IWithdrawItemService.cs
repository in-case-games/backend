using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IWithdrawItemService
    {
        public Task<decimal> GetBalance(string marketName);
        public Task<ItemInfoResponse> GetItemInfo(GameItem item);
        public Task<BuyItemResponse> BuyItem(GameItem item, string tradeUrl);
        public Task<TradeInfoResponse> GetTradeInfo(UserHistoryWithdraw history);
    }
}
