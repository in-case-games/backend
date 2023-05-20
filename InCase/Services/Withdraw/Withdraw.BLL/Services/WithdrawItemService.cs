using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class WithdrawItemService : IWithdrawItemService
    {
        private readonly Dictionary<string, ITradeMarketService> _marketServices;

        public WithdrawItemService(MarketTMService tmService)
        {
            _marketServices = new()
            {
                ["tm"] = tmService,
            };
        }

        public Task<BuyItemResponse> BuyItem(GameItem item, string tradeUrl)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetBalance(string marketName)
        {
            throw new NotImplementedException();
        }

        public Task<ItemInfoResponse> GetItemInfo(GameItem item)
        {
            throw new NotImplementedException();
        }

        public Task<TradeInfoResponse> GetTradeInfo(UserHistoryWithdraw history)
        {
            throw new NotImplementedException();
        }
    }
}
