using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;

namespace Payment.BLL.Services;

public class ResponseService(ILogger<ResponseService> logger) : IResponseService
{
    private readonly HttpClient _httpClient = new();

    public async Task<T?> GetAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken);
        }

        logger.LogError($"GET - Пришел не корректный статус код{Environment.NewLine}" +
            $"{response.StatusCode}{Environment.NewLine}" +
            $"{response.RequestMessage}{Environment.NewLine}" +
            $"{response.Headers}{Environment.NewLine}" +
            $"{response.ReasonPhrase}{Environment.NewLine}" +
            $"{response.Content}{Environment.NewLine}");

        throw new UnknownException("Внутренняя ошибка");
    }

    public async Task<T?> PostAsync<T, TK>(string uri, TK body, CancellationToken cancellationToken = default)
    {
        var json = JsonContent.Create(body);
        var response = await _httpClient.PostAsync(uri, json, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken);
        }

        logger.LogError($"POST - Пришел не корректный статус код{Environment.NewLine}" +
            $"{response.StatusCode}{Environment.NewLine}" +
            $"{response.RequestMessage}{Environment.NewLine}" +
            $"{response.Headers}{Environment.NewLine}" +
            $"{response.ReasonPhrase}{Environment.NewLine}" +
            $"{response.Content}{Environment.NewLine}");

        throw new UnknownException("Внутренняя ошибка");
    }
}