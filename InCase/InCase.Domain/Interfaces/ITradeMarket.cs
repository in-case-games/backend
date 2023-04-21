using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Interfaces
{
    public interface ITradeMarket
    {
        public Task<decimal> GetBalance();
        public Task<ItemInfo> GetItemInfo(GameItem item);
        public Task<BuyItem> BuyItem(ItemInfo info, string tradeUrl);
        public Task<TradeInfo> GetTradeInfo(UserHistoryWithdraw withdraw);
    }
}
