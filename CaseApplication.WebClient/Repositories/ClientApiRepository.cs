using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace CaseApplication.WebClient.Repositories
{
    public class ClientApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://localhost:7053";

        public ClientApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> CreateResponseGet<T>(string urlGet) where T : new()
        {
            string url = baseUrl + urlGet;

            T gameCase = new();

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                gameCase = await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new();
            }

            return gameCase;
        }

        public async Task<HttpStatusCode> CreateResponsePost<T>(PostEntityModel<T> model)
        {
            string url = baseUrl + model.PostUrl;

            JsonContent json = JsonContent.Create(model.PostContent);

            HttpResponseMessage response = await _httpClient.PostAsync(url, json);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> CreateResponsePut<T>(PostEntityModel<T> model)
        {
            string url = baseUrl + model.PostUrl;

            JsonContent json = JsonContent.Create(model.PostContent);

            HttpResponseMessage response = await _httpClient.PutAsync(url, json);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> CreateResponseDelete(string urlDelete)
        {
            string url = baseUrl + urlDelete;
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);

            return response.StatusCode;
        }
    }
}
