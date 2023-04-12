﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using InCase.Domain.Entities;
using InCase.Domain.Common;

namespace InCase.IntegrationTests.Services
{
    public class ResponseService
    {
        private readonly HttpClient _httpClient;
        public string BaseUrl { get; set; } = "https://localhost:7102";

        public ResponseService()
        {
            _httpClient = new();
        }

        public ResponseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpStatusCode> ResponseGetStatusCode(string uri, string token = "")
        {
            _httpClient.DefaultRequestHeaders.Authorization = default;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl + uri);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> ResponsePostStatusCode<T>(string uri, T entity, string token = "")
            where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = default;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            }

            JsonContent json = JsonContent.Create(entity);

            HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl + uri, json);

            return response.StatusCode;
        }

        public async Task<T?> ResponseGet<T>(string uri, string token = "")
            where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = default;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl + uri);

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
                .ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async Task<O?> ResponsePost<T, O>(string uri, T entity, string token = "")
            where T : BaseEntity
        {
            _httpClient.DefaultRequestHeaders.Authorization = default;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            }

            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl + uri, json);

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

        public async Task<HttpStatusCode> ResponsePut<T>(string uri, T entity, string token = "")
            where T : BaseEntity
        {
            _httpClient.DefaultRequestHeaders.Authorization = default;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            }

            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PutAsync(BaseUrl + uri, json);

            return response.StatusCode;
        }
        public async Task<HttpStatusCode> ResponseDelete(string uri, string token = "")
        {
            _httpClient.DefaultRequestHeaders.Authorization = default;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync(BaseUrl + uri);

            return response.StatusCode;
        }
    }
}
