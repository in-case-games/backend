using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace InCase.Infrastructure.Services
{
    public class TradeMarketService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new();

        public TradeMarketService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<decimal> GetTradeMarketInfo()
        {
            string requestUrl = $"https://market.csgo.com/api/GetMoney/?key={_configuration["MarketTM:Secret"]}";

            ResponseBalanceTM? balanceTM = await TakeResponse<ResponseBalanceTM>(requestUrl);

            return balanceTM!.Money;
        }

        public async Task<ResponseBuyItemTM?> BuyMarketItem(GameItem gameItem, string partner, string token)
        {
            string requestUrl = string.Format("https://market.{0}.com/api/Buy/{1}/?key={2}/partner={3}/token={4}",
                gameItem.Name!.ToLower(),
                gameItem.IdForPlatform,
                _configuration["MarketTM:Secret"],
                partner,
                token);

            return await TakeResponse<ResponseBuyItemTM>(requestUrl);
        }

        public async Task<ItemInfoTM?> GetMarketItemInfo(GameItem gameItem)
        {
            string requestUrl = string.Format("https://{0}.com/api/ItemInfo/{1}/ru/?key={2}",
                gameItem.Name!.ToLower(),
                gameItem.IdForPlatform,
                _configuration["MarketTM:Secret"]);

            return await TakeResponse<ItemInfoTM>(requestUrl);
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
    }
}
