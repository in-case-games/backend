using System.Net.Http.Json;
using System.Text.Json;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.Services
{
    public class ResponseService : IResponseService
    {
        private readonly HttpClient _httpClient;

        public ResponseService()
        {
            _httpClient = new();
        }

        public async Task<T?> GetAsync<T>(string uri, CancellationToken cancellation = default)
        {
            var response = await _httpClient.GetAsync(uri, cancellation);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage! +
                    response.Headers +
                    response.ReasonPhrase! +
                    response.Content);
            }

            return await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellation);
        }
    }
}
