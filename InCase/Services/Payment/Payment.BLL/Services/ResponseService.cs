using System.Net.Http.Json;
using System.Text.Json;

namespace InCase.Infrastructure.Services
{
    public class ResponseService
    {
        private readonly HttpClient _httpClient;

        public ResponseService()
        {
            _httpClient = new();
        }

        public async Task<O?> ResponsePost<T, O>(string uri, T entity)
            where T : new()
        {
            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PostAsync(uri, json);

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
                .ReadFromJsonAsync<O>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}
