﻿using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class MarketTmService : ITradeMarketService
    {
        private readonly IConfiguration _cfg;
        private readonly IResponseService _responseService;
        private readonly Dictionary<string, string> _domainUri = new()
        {
            ["csgo"] = "https://market.csgo.com",
            ["dota2"] = "https://market.dota2.net"
        };

        private readonly Dictionary<string, string> _tradeStatuses = new()
        {
            ["h_1"] = "purchase",
            ["h_2"] = "given",
            ["h_5"] = "cancel",
            ["t_1"] = "purchase",
            ["t_2"] = "purchase",
            ["t_3"] = "transfer",
            ["t_4"] = "transfer",
        };

        public MarketTmService(IConfiguration cfg, IResponseService responseService)
        {
            _cfg = cfg;
            _responseService = responseService;
        }

        public async Task<BalanceMarketResponse> GetBalanceAsync(CancellationToken cancellation = default)
        {
            var uri = $"{_domainUri["csgo"]}/api/GetMoney/?key={_cfg["MarketTM:Secret"]}";
            
            var response = await _responseService.GetAsync<BalanceTmResponse>(uri, cancellation);

            return new BalanceMarketResponse { Balance = response!.MoneyKopecks * 0.01M };
        }

        public async Task<ItemInfoResponse> GetItemInfoAsync(string idForMarket, string game, 
            CancellationToken cancellation = default)
        {
            var id = idForMarket.Replace("-", "_");
            var uri = $"{_domainUri[game]}/api/ItemInfo/{id}/ru/?key={_cfg["MarketTM:Secret"]}";

            var info = await _responseService.GetAsync<ItemInfoTmResponse>(uri, cancellation);

            return new ItemInfoResponse
            {
                Id = id,
                Count = info!.Offers!.Count,
                PriceKopecks = int.Parse(info.MinPrice!),
            };
        }

        public async Task<TradeInfoResponse> GetTradeInfoAsync(UserHistoryWithdraw history, 
            CancellationToken cancellation = default)
        {
            var name = history.Item!.Game!.Name!;
            var id = history.InvoiceId!;

            var info = new TradeInfoResponse
            {
                Id = id,
                Item = history.Item
            };

            try
            {
                var tradeUrl = $"{_domainUri[name]}/api/Trades/?key={_cfg["MarketTM:Secret"]}";

                var trades = await _responseService.GetAsync<List<TradeInfoTmResponse>>(tradeUrl, cancellation) ?? new();

                var status = trades!.First(f => f.Id == id).Status!;

                info.Status = _tradeStatuses["t_" + status];
            }
            catch(Exception)
            {
                var start = ((DateTimeOffset)history.Date).ToUnixTimeSeconds();
                var end = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                var historyUrl = $"{_domainUri[name]}/api/OperationHistory/{start}/{end}/?key={_cfg["MarketTM:Secret"]}";

                var answer = await _responseService.GetAsync<AnswerOperationHistoryTmResponse>(historyUrl, cancellation);

                var status = answer!.Histories!.First(f => f.Id == id).Status!;

                info.Status = _tradeStatuses["h_" + status];
            }

            return info;
        }

        public async Task<BuyItemResponse> BuyItemAsync(ItemInfoResponse info, string trade, 
            CancellationToken cancellation = default)
        {
            var price = info.PriceKopecks;
            var name = info.Item.Game!.Name!;
            var id = info.Item.IdForMarket!.Replace("-", "_");
            var split = trade.Split("&");
            var partner = split[0].Split("=")[1];
            var token = split[1].Split("=")[1];

            var url = $"{_domainUri[name]}/api/Buy/{id}/{price}//?key={_cfg["MarketTM:Secret"]}&partner={partner}&token={token}";

            var response = await _responseService.GetAsync<BuyItemTmResponse>(url, cancellation);
            
            return new BuyItemResponse { Id = response!.Id! };
        }

        private class AnswerOperationHistoryTmResponse
        {
            [JsonPropertyName("success")] public bool Success { get; set; }
            [JsonPropertyName("history")] public List<OperationHistoryTmResponse>? Histories { get; set; }
        }
        private class OperationHistoryTmResponse
        {
            [JsonPropertyName("item")] public string? Id { get; set; }
            [JsonPropertyName("h_id")] public string? HistoryId { get; set; }
            [JsonPropertyName("market_name")] public string? MarketName { get; set; }
            [JsonPropertyName("stage")]
            public string? Status { get; set; }

        }
        private class TradeInfoTmResponse
        {
            [JsonPropertyName("ui_id")] 
            public string? Id { get; set; }
            [JsonPropertyName("ui_status")]
            public string? Status { get; set; }

        }
        private class BuyItemTmResponse
        {
            [JsonPropertyName("result")] public string? Result { get; set; }
            [JsonPropertyName("id")] public string? Id { get; set; }
        }
        private class BalanceTmResponse
        {
            [JsonPropertyName("money")] public int MoneyKopecks { get; set; }
        }
        private class ItemInfoTmResponse
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
            [JsonPropertyName("offers")] public ICollection<OfferTm>? Offers { get; set; }
            [JsonPropertyName("buy_offers")] public ICollection<BuyOfferTm>? BuyOffers { get; set; }
        }
        private class BuyOfferTm
        {
            [JsonPropertyName("c")] public string? Count { get; set; }
            [JsonPropertyName("my_count")] public string? MyCount { get; set; }
            [JsonPropertyName("o_price")] public string? Price { get; set; }
        }
        private class OfferTm
        {
            [JsonPropertyName("price")] public string? Price { get; set; }
            [JsonPropertyName("count")] public string? Count { get; set; }
            [JsonPropertyName("my_count")] public string? MyCount { get; set; }
        }
    }
}
