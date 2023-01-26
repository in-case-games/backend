using CaseApplication.DomainLayer.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CaseApplication.WebClient.Services
{
    public class ResponseHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://localhost:7053";

        public ResponseHelper()
        {
            _httpClient = new HttpClient();
        }

        public ResponseHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpStatusCode> ResponseGetStatusCode(string uri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrl + uri);

            return response.StatusCode;
        }
        public async Task<T> ResponseGet<T>(string uri) where T: new()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrl + uri);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage!.ToString() + 
                    response.Headers.ToString() +
                    response.ReasonPhrase!.ToString() + 
                    response.Content.ToString());
            }

            return await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new();
        }

        public async Task<HttpStatusCode> ResponsePost<T>(string uri, T entity) where T: BaseEntity
        {
            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PostAsync(baseUrl + uri, json);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> ResponsePut<T>(string uri, T entity) where T: BaseEntity
        {
            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PutAsync(baseUrl + uri, json);

            return response.StatusCode;
        }
        public async Task<HttpStatusCode> ResponseDelete(string uri)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUrl + uri);

            return response.StatusCode;
        }
    }
}
