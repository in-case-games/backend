using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
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

        public async Task<BalanceMarket> GetBalance()
        {
            string requestUrl = $"https://market.csgo.com/api/GetMoney/?key={_configuration["MarketTM:Secret"]}";

            try
            {
                ResponseBalanceTM? balanceTM = await _responseService.ResponseGet<ResponseBalanceTM>(requestUrl);

                return new()
                {
                    Balance = balanceTM!.MoneyKopecks * 0.01M,
                    Result = "ok"
                };
            }
            //TODO Add many check exception
            catch(Exception)
            {
                //TODO Write logs

                return new() { 
                    Balance = -1,
                    Result = "exception"
                };
            }
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
             
            try
            {
                ResponseBuyItemTM? response = await _responseService.ResponseGet<ResponseBuyItemTM>(requestUrl);

                return new()
                {
                    Id = response!.Id,
                    Result = response.Result,
                };
            }
            //TODO Add many check exception
            catch (Exception)
            {
                //Write logs

                return new()
                {
                    Id = "-1",
                    Result = "exception",
                };
            }
        }

        public async Task<ItemInfo> GetItemInfo(GameItem item)
        {
            string name = item.Game!.Name!;
            string uri = DomainUri[name];
            string id = item.IdForMarket!.Replace("-", "_");

            string requestUrl = $"{uri}/api/ItemInfo/{id}/ru/?key={_configuration["MarketTM:Secret"]}";
            
            try
            {
                ItemInfoTM? infoTM = await _responseService.ResponseGet<ItemInfoTM>(requestUrl);

                return new()
                {
                    Count = infoTM!.Offers!.Count,
                    PriceKopecks = int.Parse(infoTM.MinPrice!),
                    Item = item,
                    Result = "ok"
                };
            }
            catch (Exception)
            {
                //TODO Write logs
                return new()
                {
                    Count = 0,
                    PriceKopecks = 0,
                    Item = item,
                    Result = "exception"
                };
            }
        }

        public async Task<TradeInfo> GetTradeInfo(UserHistoryWithdraw withdraw)
        {
            string name = withdraw.Item!.Game!.Name!;
            string uri = DomainUri[name];
            string id = withdraw.IdForMarket!;

            long startTime = ((DateTimeOffset)withdraw.Date).ToUnixTimeSeconds();
            long endTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string urlTrade = $"{uri}/api/Trades/?key={_configuration["MarketTM:Secret"]}";
            string urlHistory = $"{uri}/api/OperationHistory/{startTime}/{endTime}" +
                $"/?key={_configuration["MarketTM:Secret"]}";

            ResponseTradeTM? tradeTM = null;
            ResponseOperationHistoryTM? historyTM = null;
            TradeInfo tradeInfo = new()
            {
                Id = withdraw.IdForMarket,
                Item = withdraw.Item
            };

            try
            {
                List<ResponseTradeTM>? tradesTM = await _responseService
                    .ResponseGet<List<ResponseTradeTM>>(urlTrade);
                tradeTM = tradesTM?
                    .FirstOrDefault(f => f.Id == id);
            }
            catch(Exception)
            { 
                tradeInfo.Result = "exception";
            }
            
            if (tradeTM is null)
            {
                try
                {
                    ResponseAnswerOperationHistoryTM? answer = await _responseService
                        .ResponseGet<ResponseAnswerOperationHistoryTM>(urlHistory);

                    List<ResponseOperationHistoryTM>? historiesTM = answer?.Histories;

                    historyTM = historiesTM?
                        .FirstOrDefault(f => f.Id == withdraw.IdForMarket);
                }
                catch (Exception) 
                {
                    tradeInfo.Result = "exception";
                }

                if (historyTM is null)
                    return tradeInfo;
            }

            tradeInfo.Status = (tradeTM is null) ? historyTM!.Status! : tradeTM.Status!;
            tradeInfo.Result = "ok";

            return tradeInfo;
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
