using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class MarketTMService : ITradeMarketService
    {
        private readonly IConfiguration _cfg;
        private readonly IResponseService _responseService;
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
            ["t_1"] = "purchase",
            ["t_2"] = "purchase",
            ["t_3"] = "transfer",
            ["t_4"] = "transfer",
        };

        public MarketTMService(
            IConfiguration cfg, 
            IResponseService responseService)
        {
            _cfg = cfg;
            _responseService = responseService;
        }

        public async Task<BalanceMarketResponse> GetBalanceAsync(CancellationToken cancellation = default)
        {
            string uri = $"{DomainUri["csgo"]}/api/GetMoney/?key={_cfg["MarketTM:Secret"]}";
            
            BalanceTMResponse? response = await _responseService
                .GetAsync<BalanceTMResponse>(uri, cancellation);

            return new() { Balance = response!.MoneyKopecks * 0.01M };
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(string idForMarket, string game, CancellationToken cancellation = default)
        {
            string id = idForMarket.Replace("-", "_");

            string uri = $"{DomainUri[game]}/api/ItemInfo/{id}/ru/?key={_cfg["MarketTM:Secret"]}";

            ItemInfoTMResponse? info = await _responseService
                .GetAsync<ItemInfoTMResponse>(uri, cancellation);

            return new()
            {
                Id = id,
                Count = info!.Offers!.Count,
                PriceKopecks = int.Parse(info.MinPrice!),
            };
        }

        public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, CancellationToken cancellation = default)
        {
            string name = history.Item!.Game!.Name!;
            string id = history.InvoiceId!;

            TradeInfoResponse info = new()
            {
                Id = id,
                Item = history.Item
            };

            try
            {
                string tradeUrl = $"{DomainUri[name]}/api/Trades/?key={_cfg["MarketTM:Secret"]}";

                List<TradeInfoTMResponse> trades = await _responseService
                    .GetAsync<List<TradeInfoTMResponse>>(tradeUrl, cancellation) ?? new();

                string status = trades!
                    .First(f => f.Id == id).Status!;

                info.Status = TradeStatuses["t_" + status];
            }
            catch(Exception)
            {
                long start = ((DateTimeOffset)history.Date).ToUnixTimeSeconds();
                long end = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                string historyUrl = $"{DomainUri[name]}/api/OperationHistory/{start}/{end}" +
                    $"/?key={_cfg["MarketTM:Secret"]}";

                AnswerOperationHistoryTMResponse? answer = await _responseService
                    .GetAsync<AnswerOperationHistoryTMResponse>(historyUrl, cancellation);

                string status = answer!.Histories!
                    .First(f => f.Id == id).Status!;

                info.Status = TradeStatuses["h_" + status];
            }

            return info;
        }

        public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string trade, CancellationToken cancellation = default)
        {
            int price = info.PriceKopecks;
            string name = info.Item.Game!.Name!;
            string id = info.Item.IdForMarket!.Replace("-", "_");
            string[] split = trade.Split("&");
            string partner = split[0].Split("=")[1];
            string token = split[1].Split("=")[1];

            string url = $"{DomainUri[name]}/api/Buy/{id}/{price}//?key={_cfg["MarketTM:Secret"]}" +
                $"&partner={partner}&token={token}";

            BuyItemTMResponse? response = await _responseService
                .GetAsync<BuyItemTMResponse>(url, cancellation);
            
            return new() { Id = response!.Id! };
        }

        private class AnswerOperationHistoryTMResponse
        {
            [JsonPropertyName("success")] public bool Success { get; set; }
            [JsonPropertyName("history")] public List<OperationHistoryTMResponse>? Histories { get; set; }
        }
        private class OperationHistoryTMResponse
        {
            [JsonPropertyName("item")] public string? Id { get; set; }
            [JsonPropertyName("h_id")] public string? HistoryId { get; set; }
            [JsonPropertyName("market_name")] public string? MarketName { get; set; }
            [JsonPropertyName("stage")]
            public string? Status { get; set; }

        }
        private class TradeInfoTMResponse
        {
            [JsonPropertyName("ui_id")] 
            public string? Id { get; set; }
            [JsonPropertyName("ui_status")]
            public string? Status { get; set; }

        }
        private class BuyItemTMResponse
        {
            [JsonPropertyName("result")] public string? Result { get; set; }
            [JsonPropertyName("id")] public string? Id { get; set; }
        }
        private class BalanceTMResponse
        {
            [JsonPropertyName("money")] public int MoneyKopecks { get; set; }
        }
        private class ItemInfoTMResponse
        {
            [JsonPropertyName("classid")] public string? ClassId { get; set; }
            [JsonPropertyName("instanceid")] public string? InstanceId { get; set; }
            [JsonPropertyName("our_market_instanceid")] public string? OurMarketInstanceId { get; set; }
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
    }
}
