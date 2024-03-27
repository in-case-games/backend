using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services;
public class WithdrawItemService(
    MarketTmService tmService, 
    ILogger<WithdrawItemService> logger) : IWithdrawItemService
{
    private const int NumberAttempts = 5;
    private readonly ConcurrentDictionary<string, ITradeMarketService> _marketServices = new()
    {
        ["tm"] = tmService,
    };

    public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string tradeUrl, CancellationToken cancellation = default)
    {
        var name = info.Market.Name ?? throw new BadRequestException("Название маркета пустое");

        if (!_marketServices.TryGetValue(name, out var value)) 
            throw new NotFoundException("Маркет не найден");

        var i = 0;

        while (i < NumberAttempts)
        {
            try
            {
                var item = await value.BuyItemAsync(info, tradeUrl, cancellation);

                item.Market = info.Market;

                logger.LogInformation($"The item successfully bought. ItemId: {item.Id}. MarketName: {name}");
                return item;
            }
            catch (Exception ex) 
            { 
                logger.LogError($"Не смог купить предмет - {info.Item.Id}");
                logger.LogError(ex, ex.Message);
                logger.LogError(ex, ex.StackTrace);
                i++; 
            }

            await Task.Delay(1000, cancellation);
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
            catch(Exception ex)
            {
                logger.LogError($"Не смог получить баланс маркета - {marketName}");
                logger.LogError(ex, ex.Message);
                logger.LogError(ex, ex.StackTrace);
                i++;
            }

            await Task.Delay(1000, cancellation);
        }

        throw new RequestTimeoutException("Сервис покупки предметов не отвечает");
    }

    public async Task<ItemInfoResponse> GetItemInfoAsync(GameItem item, CancellationToken cancellation = default)
    {
        var gameName = item.Game?.Name ?? throw new BadRequestException("Название игры пустое");
        var market = item.Game?.Market ?? throw new BadRequestException("Название маркета пустое");

        if (!_marketServices.TryGetValue(market.Name!, out var value))
            throw new NotFoundException("Маркет не найден");

        var i = 0;

        while (i < NumberAttempts)
        {
            try
            {
                var info = await value.GetItemInfoAsync(item.IdForMarket!, gameName, cancellation);
                
                info.Item = item;
                info.Market = market;
                
                return info;
            }
            catch (Exception ex)
            {
                logger.LogError($"Не смог получить информацию о предмете - {item.Id}");
                logger.LogError(ex, ex.Message);
                logger.LogError(ex, ex.StackTrace);
                i++;
            }

            await Task.Delay(1000, cancellation);
        }
        
        throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
    }

    public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, CancellationToken cancellation = default)
    {
        var name = history.Market?.Name ?? throw new BadRequestException("Название маркета пустое");

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
            catch(Exception ex)
            {
                logger.LogError($"Не смог получить информацию о выводе - {history.Item!.Id}");
                logger.LogError(ex, ex.Message);
                logger.LogError(ex, ex.StackTrace);
                i++;
            }

            await Task.Delay(1000, cancellation);
        }

        throw new RequestTimeoutException("Сервис покупки предмета не отвечает");
    }
}