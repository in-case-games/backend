using Microsoft.Extensions.Logging;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class WithdrawItemService : IWithdrawItemService
    {
        private const int NumberAttempts = 5;
        private readonly Dictionary<string, ITradeMarketService> _marketServices;
        private readonly ILogger<WithdrawItemService> _logger;

        public WithdrawItemService(MarketTMService tmService, ILogger<WithdrawItemService> logger)
        {
            _logger = logger;
            _marketServices = new()
            {
                ["tm"] = tmService,
            };
        }

        public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl)
        {
            var name = info.Market.Name!;

            if (!_marketServices.ContainsKey(name)) 
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var item = await _marketServices[name]
                        .BuyItemAsync(info, tradeUrl);

                    item.Market = info.Market;

                    _logger.LogInformation($"The item successfully bougth. ItemId: {item.Id}. MarketName: {name}");
                    return item;
                }
                catch (Exception) 
                { 
                    i++; 
                }

                await Task.Delay(2000);
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<BalanceMarketResponse> GetBalanceAsync(string marketName)
        {
            if (!_marketServices.ContainsKey(marketName))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    return await _marketServices[marketName].GetBalanceAsync();
                }
                catch(Exception)
                {
                    i++;
                }

                await Task.Delay(2000);
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(GameItem item)
        {
            var gameName = item.Game!.Name!;
            var market = item.Game!.Market!;

            if (!_marketServices.ContainsKey(market.Name!))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var info = await _marketServices[market.Name!]
                        .GetItemInfoAsync(item.IdForMarket!, gameName);
                    
                    info!.Item = item;
                    info!.Market = market;
                    
                    return info;
                }
                catch (Exception)
                {
                    i++;
                }

                await Task.Delay(2000);
            }
            
            throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
        }

        public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history)
        {
            var name = history.Market!.Name!;

            if (!_marketServices.ContainsKey(name))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var info = await _marketServices[name]
                        .GetTradeInfoAsync(history);

                    info.Item = history.Item;

                    return info;
                }
                catch(Exception)
                {
                    i++;
                }

                await Task.Delay(2000);
            }

            throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
        }
    }
}
