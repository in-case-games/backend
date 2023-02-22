using CaseApplication.Domain.Entities.Payment;
using CaseApplication.Domain.Entities.Resources;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace CaseApplication.Infrastructure.Services
{
    public class MarketTMService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new();

        public MarketTMService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<decimal> GetBalanceTM()
        {
            string requestUrl = $"https://market.csgo.com/api/GetMoney/?key={_configuration["MarketTM:Secret"]}";

            ResponseBalanceTM? balanceTM = await TakeResponse<ResponseBalanceTM>(requestUrl);

            return balanceTM!.Money;
        }

        public async Task<ResponseBuyItemTM?> BuyItemMarket(GameItem gameItem, string partner, string token)
        {
            string requestUrl = string.Format("https://{0}.com/api/Buy/{1}/?key={2}/partner={3}/token={4}",
                gameItem.GameName!.ToLower(),
                gameItem.GameItemIdForPlatform,
                _configuration["MarketTM:Secret"],
                partner,
                token);

            return await TakeResponse<ResponseBuyItemTM>(requestUrl);
        }

        public async Task<ItemInfoTM?> GetItemInfoMarket(GameItem gameItem)
        {
            string requestUrl = string.Format("https://{0}.com/api/ItemInfo/{1}/ru/?key={2}",
                gameItem.GameName!.ToLower(),
                gameItem.GameItemIdForPlatform,
                _configuration["MarketTM:Secret"]);

            return await TakeResponse<ItemInfoTM>(requestUrl);
        }
        public async Task<T> TakeResponse<T>(string url)
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

            return responseEntity!;
        }
    }
}
