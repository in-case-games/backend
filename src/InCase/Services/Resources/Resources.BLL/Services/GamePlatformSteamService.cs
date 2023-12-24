using Microsoft.Extensions.Configuration;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Resources.BLL.Services
{
    public class GamePlatformSteamService : IGamePlatformService
    {
        private const int NumberAttempts = 5;

        private readonly ILogger<GamePlatformSteamService> _logger;
        private readonly IConfiguration _cfg;
        private readonly IResponseService _responseService;
        private readonly Dictionary<string, string> _domainUri = new()
        {
            ["csgo"] = "https://market.csgo.com",
            ["dota2"] = "https://market.dota2.net"
        };
        private readonly Dictionary<string, string> _appId = new()
        {
            ["csgo"] = "730",
            ["dota2"] = "570"
        };

        public GamePlatformSteamService(ILogger<GamePlatformSteamService> logger, IConfiguration cfg, IResponseService responseService)
        {
            _logger = logger;
            _cfg = cfg;
            _responseService = responseService;
        }

        public async Task<ItemCostResponse> GetAdditionalMarketAsync(string idForMarket, string game, 
            CancellationToken cancellation = default)
        {
            var id = idForMarket.Replace("-", "_");
            var uri = $"{_domainUri[game]}/api/ItemInfo/{id}/ru/?key={_cfg["MarketTM:Secret"]}";

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var info = await _responseService.GetAsync<ItemInfoTmResponse>(uri, cancellation);

                    return info?.Cost is null ?
                        new ItemCostResponse { Success = false, Cost = 0M } :
                        new ItemCostResponse { Success = true, Cost = decimal.Parse(info.Cost!) / 100 };
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Не смог получить стоимость предмета additional market - {idForMarket}");
                    _logger.LogError(ex, ex.Message);
                    _logger.LogError(ex, ex.StackTrace);
                    i++;
                }

                await Task.Delay(1000, cancellation);
            }

            return new ItemCostResponse { Success = false, Cost = 0M };
        }

        public async Task<ItemCostResponse> GetOriginalMarketAsync(string hashName, string game, 
            CancellationToken cancellation = default)
        {
            var uri = $"https://steamcommunity.com/market/priceoverview/?" +
                $"currency=5&" +
                $"country=ru&" +
                $"appid={_appId[game]}&" +
                $"market_hash_name={hashName}&" +
                $"format=json";

            var i = 0;

            while (i < NumberAttempts)
            {
                try
                {
                    var info = await _responseService.GetAsync<ItemInfoSteamResponse>(uri, cancellation);

                    if (info is null || !info.Success || info.Cost is null)
                    {
                        return new ItemCostResponse { Success = false, Cost = 0M };
                    }

                    var temp = info!.Cost!.Replace(" pуб.", "");
                    var cost = decimal.Parse(temp);

                    if (temp != cost.ToString()) cost /= 100;

                    return new ItemCostResponse { Success = true, Cost = cost };
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Не смог получить стоимость предмета original market - {hashName}");
                    _logger.LogError(ex, ex.Message);
                    _logger.LogError(ex, ex.StackTrace);
                    i++;
                }

                await Task.Delay(1000, cancellation);
            }

            return new ItemCostResponse { Success = false, Cost = 0M };
        }

        private class ItemInfoTmResponse
        {
            [JsonPropertyName("min_price")] public string? Cost { get; set; }
        }

        private class ItemInfoSteamResponse
        {
            [JsonPropertyName("success")] public bool Success { get; set; } = false;
            [JsonPropertyName("lowest_price")] public string? Cost { get; set; }
        }
    }
}
