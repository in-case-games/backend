using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class WithdrawItemService : IWithdrawItemService
    {
        private const int NumberAttempts = 5;
        private readonly ConcurrentDictionary<string, ITradeMarketService> _marketServices;
        private readonly ILogger<WithdrawItemService> _logger;

        public WithdrawItemService(MarketTmService tmService, ILogger<WithdrawItemService> logger)
        {
            _logger = logger;
            _marketServices = new ConcurrentDictionary<string, ITradeMarketService>
            {
                ["tm"] = tmService,
            };
        }

        public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl, CancellationToken cancellation = default)
        {
            var name = info.Market.Name!;

            if (!_marketServices.TryGetValue(name, out var value)) 
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var item = await value.BuyItemAsync(info, tradeUrl, cancellation);

                    item.Market = info.Market;

                    _logger.LogInformation($"The item successfully bought. ItemId: {item.Id}. MarketName: {name}");
                    return item;
                }
                catch (Exception) 
                { 
                    i++; 
                }

                await Task.Delay(200, cancellation);
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<BalanceMarketResponse> GetBalanceAsync(string marketName, CancellationToken cancellation = default)
        {
            if (!_marketServices.TryGetValue(marketName, out var value))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    return await value.GetBalanceAsync(cancellation);
                }
                catch(Exception)
                {
                    i++;
                }

                await Task.Delay(200, cancellation);
            }

            throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(GameItem item, CancellationToken cancellation = default)
        {
            var gameName = item.Game!.Name!;
            var market = item.Game!.Market!;

            if (!_marketServices.TryGetValue(market.Name!, out var value))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var info = await value.GetItemInfoAsync(item.IdForMarket!, gameName, cancellation);
                    
                    info!.Item = item;
                    info!.Market = market;
                    
                    return info;
                }
                catch (Exception)
                {
                    i++;
                }

                await Task.Delay(200, cancellation);
            }
            
            throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
        }

        public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, CancellationToken cancellation = default)
        {
            var name = history.Market!.Name!;

            if (!_marketServices.TryGetValue(name, out var value))
                throw new NotFoundException("Маркет не найден");

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var info = await value.GetTradeInfoAsync(history, cancellation);

                    info.Item = history.Item;

                    return info;
                }
                catch(Exception)
                {
                    i++;
                }

                await Task.Delay(200, cancellation);
            }

            throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
        }
    }
}
