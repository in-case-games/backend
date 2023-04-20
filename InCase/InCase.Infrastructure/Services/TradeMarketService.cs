using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InCase.Infrastructure.Services
{
    public class TradeMarketService : ITradeMarket
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new();

        public TradeMarketService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<decimal> GetBalance()
        {
            string requestUrl = $"https://market.csgo.com/api/GetMoney/?key={_configuration["MarketTM:Secret"]}";

            ResponseBalanceTM? balanceTM = await TakeResponse<ResponseBalanceTM>(requestUrl);

            return balanceTM!.Money;
        }

        public async Task<BuyItem> BuyItem(GameItem gameItem, string tradeUrl)
        {
            string requestUrl = string.Format("https://market.{0}.com/api/Buy/{1}/?key={2}/partner={3}/token={4}",
                gameItem.Name!.ToLower(),
                gameItem.IdForPlatform,
                _configuration["MarketTM:Secret"],
                tradeUrl,
                tradeUrl);

            ResponseBuyItemTM response = (await TakeResponse<ResponseBuyItemTM>(requestUrl))!;

            BuyItem item = new()
            {
                Id = response.BuyId,
                Result = response.Result,
            };

            return item;
        }

        public async Task<ItemInfo> GetItemInfo(GameItem gameItem)
        {
            string requestUrl = string.Format("https://{0}.com/api/ItemInfo/{1}/ru/?key={2}",
                gameItem.Name!.ToLower(),
                gameItem.IdForPlatform,
                _configuration["MarketTM:Secret"]);

            ItemInfoTM infoTM = (await TakeResponse<ItemInfoTM>(requestUrl))!;

            ItemInfo info = new()
            {
                Count = infoTM.Offers!.Count,
                Price = decimal.Parse(infoTM.MinPrice!),
                Item = gameItem
            };

            return info;
        }
        public async Task<T?> TakeResponse<T>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage! +
                    response.Headers +
                    response.ReasonPhrase! +
                    response.Content);
            }

            T? responseEntity = await response.Content
                .ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return responseEntity;
        }

        private class ResponseBuyItemTM
        {
            [JsonPropertyName("result")] public string? Result { get; set; }
            [JsonPropertyName("id")] public int BuyId { get; set; }
        }
        private class ResponseBalanceTM
        {
            [JsonPropertyName("money")] public decimal Money { get; set; }
        }
        private class BuyOfferTM
        {
            [JsonPropertyName("c")] public string? Count { get; set; }
            [JsonPropertyName("my_count")] public string? MyCount { get; set; }
            [JsonPropertyName("o_price")] public string? Price { get; set; }
        }
        private class OfferTM
        {
            [JsonPropertyName("price")] public string? Price { get; set; }
            [JsonPropertyName("count")] public string? Count { get; set; }
            [JsonPropertyName("my_count")] public string? MyCount { get; set; }
        }
        private class ItemInfoTM
        {
            [JsonPropertyName("classid")] public int ClassId { get; set; }
            [JsonPropertyName("instanceid")] public int InstanceId { get; set; }
            [JsonPropertyName("our_market_instanceid")] public int? OurMarketInstanceId { get; set; }
            [JsonPropertyName("market_name")] public string? MarketName { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
            [JsonPropertyName("market_hash_name")] public string? MarketHashName { get; set; }
            [JsonPropertyName("rarity")] public string? Rarity { get; set; }
            [JsonPropertyName("quality")] public string? Quality { get; set; }
            [JsonPropertyName("type")] public string? Type { get; set; }
            [JsonPropertyName("mtype")] public string? MType { get; set; }
            [JsonPropertyName("slot")] public string? Slot { get; set; }
            [JsonPropertyName("stickers")] public string? Stickers { get; set; }
            [JsonPropertyName("min_price")] public string? MinPrice { get; set; }
            [JsonPropertyName("offers")] public ICollection<OfferTM>? Offers { get; set; }
            [JsonPropertyName("buy_offers")] public ICollection<BuyOfferTM>? BuyOffers { get; set; }
        }
    }
}
