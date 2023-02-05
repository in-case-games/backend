using CaseApplication.DomainLayer.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CaseApplication.WebClient.Services
{
    public class ResponseHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7053";

        public ResponseHelper()
        {
            _httpClient = new HttpClient();
        }
        public ResponseHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpStatusCode> ResponseGetStatusCode(string uri, string token = "")
        {
            _httpClient.DefaultRequestHeaders
                .Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + uri);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> ResponsePostStatusCode<T>(string uri, T entity, string token = "")
            where T : BaseEntity
        {
            _httpClient.DefaultRequestHeaders
                .Add("Authorization", "Bearer " + token);

            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + uri, json);

            return response.StatusCode;
        }

        public async Task<T> ResponseGet<T>(string uri, string token = "") 
            where T: new()
        {
            _httpClient.DefaultRequestHeaders
                .Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + uri);

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
                .ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? 
                new();
        }

        public async Task<O?> ResponsePost<T, O>(string uri, T entity, string token = "")
            where T : BaseEntity
        {
            _httpClient.DefaultRequestHeaders
                .Add("Authorization", "Bearer " + token);

            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + uri, json);

            return await response.Content
                .ReadFromJsonAsync<O>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<HttpStatusCode> ResponsePut<T>(string uri,T entity, string token = "") 
            where T: BaseEntity
        {
            _httpClient.DefaultRequestHeaders
                .Add("Authorization", "Bearer " + token);

            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + uri, json);

            return response.StatusCode;
        }
        public async Task<HttpStatusCode> ResponseDelete(string uri, string token = "")
        {
            _httpClient.DefaultRequestHeaders
                .Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + uri);

            return response.StatusCode;
        }
    }
}
