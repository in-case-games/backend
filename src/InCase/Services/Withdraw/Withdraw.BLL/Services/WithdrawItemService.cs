using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class WithdrawItemService : IWithdrawItemService
    {
        private const int NumberAttepmts = 5;
        private readonly Dictionary<string, ITradeMarketService> _marketServices;

        public WithdrawItemService(MarketTMService tmService)
        {
            _marketServices = new()
            {
                ["tm"] = tmService,
            };
        }

        public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl)
        {
            string name = info.Market.Name!;

            if (!_marketServices.ContainsKey(name)) 
                throw new NotFoundException("Маркет не найден");

            int i = 0;

            while (i < NumberAttepmts)
            {
                try
                {
                    BuyItemResponse item = await _marketServices[name]
                        .BuyItemAsync(info, tradeUrl);

                    item.Market = info.Market;

                    return item;
                }
                catch (Exception) 
                { 
                    i++; 
                }
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<BalanceMarketResponse> GetBalanceAsync(string marketName)
        {
            if (!_marketServices.ContainsKey(marketName))
                throw new NotFoundException("Маркет не найден");

            int i = 0;

            while (i < NumberAttepmts)
            {
                try
                {
                    return await _marketServices[marketName].GetBalanceAsync();
                }
                catch(Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(GameItem item)
        {
            string gameName = item.Game!.Name!;
            GameMarket market = item.Game!.Market!;

            if (!_marketServices.ContainsKey(market.Name!))
                throw new NotFoundException("Маркет не найден");

            int i = 0;

            while (i < NumberAttepmts)
            {
                try
                {
                    ItemInfoResponse info = await _marketServices[market.Name!]
                        .GetItemInfoAsync(item.IdForMarket!, gameName);
                    
                    info!.Item = item;
                    info!.Market = market;
                    
                    return info;
                }
                catch (Exception)
                {
                    i++;
                }
            }
            
            throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
        }

        public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history)
        {
            string name = history.Market!.Name!;

            if (!_marketServices.ContainsKey(name))
                throw new NotFoundException("Маркет не найден");

            int i = 0;

            while (i < NumberAttepmts)
            {
                try
                {
                    TradeInfoResponse info = await _marketServices[name]
                        .GetTradeInfoAsync(history);

                    info.Item = history.Item;

                    return info;
                }
                catch(Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
        }
    }
}
