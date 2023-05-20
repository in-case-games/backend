using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class MarketTMService : ITradeMarketService
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
            ["t_1"] = "purchase",
            ["t_2"] = "purchase",
            ["t_3"] = "purchase",
            ["t_4"] = "transfer"
        };

        public MarketTMService(
            IConfiguration configuration, 
            ResponseService responseService)
        {
            _configuration = configuration;
            _responseService = responseService;
        }

        public async Task<decimal> GetBalance()
        {
            string uri = DomainUri["csgo"];
            string url = $"/api/GetMoney/?key={_configuration["MarketTM:Secret"]}";
            
            ResponseBalanceTM? response = await _responseService
                .ResponseGet<ResponseBalanceTM>(uri + url);

            return response!.MoneyKopecks * 0.01M;
        }

        public async Task<ItemInfoResponse> GetItemInfo(string idForMarket, string game)
        {
            string uri = DomainUri[game];
            string id = idForMarket.Replace("-", "_");

            string url = $"{uri}/api/ItemInfo/{id}/ru/?key={_configuration["MarketTM:Secret"]}";
            
            ItemInfoTM? info = await _responseService
                .ResponseGet<ItemInfoTM>(url);

            return new()
            {
                Count = info!.Offers!.Count,
                PriceKopecks = int.Parse(info.MinPrice!),
            };
        }

        public async Task<TradeInfoResponse> GetTradeInfo(UserHistoryWithdraw history)
        {
            string name = history.Item!.Game!.Name!;
            string uri = DomainUri[name];
            string id = history.InvoiceId!;

            TradeInfoResponse info = new()
            {
                Id = id,
                Item = history.Item
            };

            try
            {
                string tradeUrl = $"{uri}/api/Trades/?key={_configuration["MarketTM:Secret"]}";

                List<ResponseTradeTM>? trades = await _responseService
                    .ResponseGet<List<ResponseTradeTM>>(tradeUrl);

                ResponseTradeTM trade = trades!
                    .First(f => f.Id == id);

                info.Status = TradeStatuses[trade.Status!];
            }
            catch(Exception)
            {
                long start = ((DateTimeOffset)history.Date).ToUnixTimeSeconds();
                long end = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                string historyUrl = $"{uri}/api/OperationHistory/{start}/{end}" +
                    $"/?key={_configuration["MarketTM:Secret"]}";

                ResponseAnswerOperationHistoryTM? answer = await _responseService
                    .ResponseGet<ResponseAnswerOperationHistoryTM>(historyUrl);

                List<ResponseOperationHistoryTM> histories = answer!.Histories!;

                ResponseOperationHistoryTM historyTM = histories!
                    .First(f => f.Id == id);

                info.Status = TradeStatuses[historyTM.Status!];
            }

            return info;
        }

        public async Task<BuyItemResponse> BuyItem(ItemInfoResponse info, string trade)
        {
            int price = info.PriceKopecks;
            string name = info.Item.Game!.Name!;
            string uri = DomainUri[name];
            string id = info.Item.IdForMarket!.Replace("-", "_");
            string[] split = trade.Split("&");
            string partner = split[0].Split("=")[1];
            string token = split[1].Split("=")[1];

            string url = $"{uri}/api/Buy/{id}/{price}//?key={_configuration["MarketTM:Secret"]}" +
                $"&partner={partner}&token={token}";
            
            ResponseBuyItemTM? response = await _responseService
                .ResponseGet<ResponseBuyItemTM>(url);
            
            return new()
            {
                Id = response!.Id!
            };
        }

        private class ResponseAnswerOperationHistoryTM
        {
            [JsonPropertyName("success")] public bool Success { get; set; }
            [JsonPropertyName("history")] public List<ResponseOperationHistoryTM>? Histories { get; set; }
        }
        private class ResponseOperationHistoryTM
        {
            private string? _status;

            [JsonPropertyName("item")] public string? Id { get; set; }
            [JsonPropertyName("h_id")] public string? HistoryId { get; set; }
            [JsonPropertyName("market_name")] public string? MarketName { get; set; }
            [JsonPropertyName("stage")]
            public string? Status
            {
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
            [JsonPropertyName("ui_status")]
            public string? Status
            {
                get => _status;
                set
                {
                    _status = "t_" + value;
                }
            }
            [JsonPropertyName("ui_price")] public decimal Price { get; set; }
            [JsonPropertyName("position")] public string? Position { get; set; }
            [JsonPropertyName("left")] public string? Left { get; set; }

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
    }
}
