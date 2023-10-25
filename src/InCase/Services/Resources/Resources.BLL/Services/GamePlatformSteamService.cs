﻿using Microsoft.Extensions.Configuration;
using Resources.BLL.Interfaces;
using Resources.BLL.Models;
using System.Text.Json.Serialization;

namespace Resources.BLL.Services
{
    public class GamePlatformSteamService : IGamePlatformService
    {
        private readonly IConfiguration _cfg;
        private readonly IResponseService _responseService;
        private readonly Dictionary<string, string> DomainUri = new()
        {
            ["csgo"] = "https://market.csgo.com",
            ["dota2"] = "https://market.dota2.net"
        };
        private readonly Dictionary<string, string> AppId = new()
        {
            ["csgo"] = "730",
            ["dota2"] = "570"
        };

        public GamePlatformSteamService(IConfiguration cfg, IResponseService responseService)
        {
            _cfg = cfg;
            _responseService = responseService;
        }

        public async Task<ItemCostResponse> GetAdditionalMarketAsync(string idForMarket, string game)
        {
            string id = idForMarket.Replace("-", "_");

            string uri = $"{DomainUri[game]}/api/ItemInfo/{id}/ru/?key={_cfg["MarketTM:Secret"]}";

            try
            {
                ItemInfoTMResponse? info = await _responseService
                    .GetAsync<ItemInfoTMResponse>(uri);

                return (info is null || info.Cost is null) ?
                    new() { Success = false, Cost = 0M } :
                    new() { Success = true, Cost = decimal.Parse(info.Cost!) / 100 };
            }
            catch(Exception)
            {
                return new() { Success = false, Cost = 0M };
            }
        }

        public async Task<ItemCostResponse> GetOriginalMarketAsync(string hashName, string game)
        {
            string uri = $"https://steamcommunity.com/market/priceoverview/?" +
                $"currency=5&" +
                $"country=ru&" +
                $"appid={AppId[game]}&" +
                $"market_hash_name={hashName}&" +
                $"format=json";

            try
            {
                ItemInfoSteamResponse? info = await _responseService
                    .GetAsync<ItemInfoSteamResponse>(uri);
                if ((info is null || !info.Success || info.Cost is null))
                    return new() { Success = false, Cost = 0M };
                else
                {
                    string temp = info!.Cost!.Replace(" pуб.", "");
                    decimal cost = decimal.Parse(temp);

                    if (temp != cost.ToString()) cost /= 100;

                    return new() { Success = true, Cost = cost };
                }
            }
            catch (Exception)
            {
                return new() { Success = false, Cost = 0M };
            }
        }

        private class ItemInfoTMResponse
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
