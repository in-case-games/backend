using Microsoft.Extensions.Configuration;
using Resources.BLL.Interfaces;
using System.Text.Json.Serialization;

namespace Resources.BLL.Services
{
    public class GamePlatformSteamService : IGamePlatformService
    {
        private readonly IResponseService _responseService;
        private readonly Dictionary<string, string> AppId = new()
        {
            ["csgo"] = "730",
            ["dota2"] = "570"
        };

        public GamePlatformSteamService(IResponseService responseService)
        {
            _responseService = responseService;
        }

        public async Task<decimal> GetItemCostAsync(string hashName, string game)
        {
            string uri = $"https://steamcommunity.com/market/priceoverview/?" +
                $"currency=5&" +
                $"country=ru&" +
                $"appid={AppId[game]}&" +
                $"market_hash_name={hashName}&" +
                $"format=json";

            ItemCostResponse? response = await _responseService.GetAsync<ItemCostResponse>(uri);

            string ammount = response!.Cost!.Replace(" pуб.", "");

            //TODO if item no steam check price tm

            return decimal.Parse(ammount);
        }


        private class ItemCostResponse
        {
            [JsonPropertyName("lowest_price")] public string? Cost { get; set; }
        }
    }
}
