using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InCase.Infrastructure.Services
{
    public class TradeMarketService : ITradeMarket
    {
        private readonly IConfiguration _configuration;
        private readonly ResponseService _responseService;
        private readonly Dictionary<string, string> DomainUri = new()
        {
            ["csgo"] = "https://market.csgo.com",
            ["dota2"] = "https://market.dota2.net"
        };

        private readonly Dictionary<string, string> TradeStatuses = new()
        {
            ["h_1"] = "purchase",
            ["h_2"] = "given",
            ["h_5"] = "cancel",
            ["t_1"] = "waiting",
            ["t_2"] = "waiting",
            ["t_3"] = "waiting",
            ["t_4"] = "transfer"
        };

        public TradeMarketService(IConfiguration configuration, ResponseService responseService)
        {
            _configuration = configuration;
            _responseService = responseService;
        }

        public async Task<decimal> GetBalance()
        {
            string requestUrl = $"https://market.csgo.com/api/GetMoney/?key={_configuration["MarketTM:Secret"]}";

            ResponseBalanceTM? balanceTM = await _responseService.ResponseGet<ResponseBalanceTM>(requestUrl);

            if (balanceTM is null)
                throw new Exception("Затычка");

            return balanceTM.MoneyKopecks * 0.01M;
        }

        public async Task<BuyItem> BuyItem(ItemInfo info, string tradeUrl)
        {
            int price = info.PriceKopecks;
            string name = info.Item.Game!.Name!;
            string uri = DomainUri[name];
            string id = info.Item.IdForMarket!.Replace("-", "_");
            string[] splitTrade = tradeUrl.Split("&");
            string partner = splitTrade[0].Split("=")[1];
            string token = splitTrade[1].Split("=")[1];

            string requestUrl = $"{uri}/api/Buy/{id}/{price}//?key={_configuration["MarketTM:Secret"]}" +
                $"&partner={partner}&token={token}";

            ResponseBuyItemTM? response = await _responseService.ResponseGet<ResponseBuyItemTM>(requestUrl);

            if (response is null)
                throw new Exception("Затычка");

            BuyItem item = new()
            {
                Id = response.Id,
                Result = response.Result,
            };

            return item;
        }

        public async Task<ItemInfo> GetItemInfo(GameItem item)
        {
            string name = item.Game!.Name!;
            string uri = DomainUri[name];
            string id = item.IdForMarket!.Replace("-", "_");

            string requestUrl = $"{uri}/api/ItemInfo/{id}/ru/?key={_configuration["MarketTM:Secret"]}";

            ItemInfoTM? infoTM = await _responseService.ResponseGet<ItemInfoTM>(requestUrl);

            if (infoTM is null)
                throw new Exception("Затычка");

            ItemInfo info = new()
            {
                Count = infoTM.Offers!.Count,
                PriceKopecks = int.Parse(infoTM.MinPrice!),
                Item = item
            };

            return info;
        }

        public async Task<TradeInfo> GetTradeInfo(UserHistoryWithdraw withdraw)
        {
            string name = withdraw.Item!.Game!.Name!;
            string uri = DomainUri[name];
            string id = withdraw.IdForMarket!;

            string requestUrl = $"{uri}/api/Trades/?key={_configuration["MarketTM:Secret"]}";

            List<ResponseTradeTM>? tradesTM = await _responseService
                .ResponseGet<List<ResponseTradeTM>>(requestUrl);

            if(tradesTM is null)
                throw new Exception("Затычка");

            ResponseTradeTM? tradeTM = tradesTM
                .FirstOrDefault(f => f.Id == id);

            if(tradeTM is null)
            {
                long startTime = ((DateTimeOffset)withdraw.Date).ToUnixTimeSeconds();
                long endTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                requestUrl = $"{uri}/api/OperationHistory/{startTime}/{endTime}" +
                    $"/?key={_configuration["MarketTM:Secret"]}";

                List<ResponseOperationHistoryTM>? historiesTM = await _responseService
                    .ResponseGet<List<ResponseOperationHistoryTM>>(requestUrl);

                if (historiesTM is null)
                    throw new Exception("Затычка");

                ResponseOperationHistoryTM? historyTM = historiesTM
                    .FirstOrDefault(f => f.Id == withdraw.IdForMarket);

                if(historyTM is null)
                    throw new Exception("Затычка");

                return new()
                {
                    Id = withdraw.IdForMarket,
                    Item = withdraw.Item,
                    Status = TradeStatuses[historyTM.Status!]
                };
            }

            return new()
            {
                Id = withdraw.IdForMarket,
                Item = withdraw.Item,
                Status = TradeStatuses[tradeTM.Status!]
            };
        }

        private class ResponseOperationHistoryTM
        {
            private string? _status;

            [JsonPropertyName("h_id")] public string? Id { get; set; }
            [JsonPropertyName("h_event")] public string? Type { get; set; }
            [JsonPropertyName("stage")] public string? Status {
                get => _status;
                set
                {
                    _status = "h_" + value;
                }
            }
        }
        private class ResponseTradeTM
        {
            private string? _status;

            [JsonPropertyName("ui_id")] public string? Id { get; set; }
            [JsonPropertyName("ui_status")] public string? Status
            {
                get => _status;
                set
                {
                    _status = "t_" + value;
                }
            }
            [JsonPropertyName("ui_price")] public decimal Price { get; set; }
            [JsonPropertyName("position")] public int Position { get; set; }
            [JsonPropertyName("left")] public int Left { get; set; }

        }
        private class ResponseBuyItemTM
        {
            [JsonPropertyName("result")] public string? Result { get; set; }
            [JsonPropertyName("id")] public string? Id { get; set; }
        }
        private class ResponseBalanceTM
        {
            [JsonPropertyName("money")] public int MoneyKopecks { get; set; }
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
