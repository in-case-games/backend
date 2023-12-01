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

        public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl, CancellationToken cancellation = default)
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
                        .BuyItemAsync(info, tradeUrl, cancellation);

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

        public async Task<BalanceMarketResponse> GetBalanceAsync(string marketName, CancellationToken cancellation = default)
        {
            if (!_marketServices.ContainsKey(marketName))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    return await _marketServices[marketName].GetBalanceAsync(cancellation);
                }
                catch(Exception)
                {
                    i++;
                }

                await Task.Delay(2000);
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(GameItem item, CancellationToken cancellation = default)
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
                        .GetItemInfoAsync(item.IdForMarket!, gameName, cancellation);
                    
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

        public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, CancellationToken cancellation = default)
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
                        .GetTradeInfoAsync(history, cancellation);

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
