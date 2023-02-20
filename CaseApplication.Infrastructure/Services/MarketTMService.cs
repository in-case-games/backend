using CaseApplication.Domain.Entities.External;
using CaseApplication.Domain.Entities.Internal;
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

        public async Task<ItemBuyTM?> BuyItemMarket(GameItem gameItem, string partner, string token)
        {
            Dictionary<string, string> requestUrls = new() {
                {
                    "csgo",
                    $"https://market.csgo.com/api/Buy/" +
                    $"{gameItem.GameItemIdForPlatform}//?" +
                    $"key={_configuration["MarketTM:Secret"]}" +
                    $"partner={partner}&" +
                    $"token={token}"
                },
                {
                    "dota2",
                    $"https://market.dota2.net/api/Buy/" +
                    $"{gameItem.GameItemIdForPlatform}//?" +
                    $"key={_configuration["MarketTM:Secret"]}&" +
                    $"partner={partner}&" +
                    $"token={token}"
                }
            };
            string requestUrl = requestUrls.FirstOrDefault(x => x.Key == gameItem.GameName).Value;

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage! +
                    response.Headers +
                    response.ReasonPhrase! +
                    response.Content);
            }

            return await response.Content
                .ReadFromJsonAsync<ItemBuyTM>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<ItemInfoTM?> GetItemInfoMarket(GameItem gameItem)
        {
            Dictionary<string, string> requestUrls = new() {
                {
                    "csgo",
                    $"https://market.csgo.com/api/ItemInfo/" +
                    $"{gameItem.GameItemIdForPlatform}/ru/?" +
                    $"key={_configuration["MarketTM:Secret"]}"
                },
                {
                    "dota2",
                    $"https://market.dota2.net/api/ItemInfo/" +
                    $"{gameItem.GameItemIdForPlatform}/ru/?" +
                    $"key={_configuration["MarketTM:Secret"]}"
                }
            };

            string requestUrl = requestUrls.FirstOrDefault(x => x.Key == gameItem.GameName).Value;

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage! +
                    response.Headers +
                    response.ReasonPhrase! +
                    response.Content);
            }

            return await response.Content
                .ReadFromJsonAsync<ItemInfoTM>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}
