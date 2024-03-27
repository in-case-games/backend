using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;

namespace Payment.BLL.Services;
public class ResponseService(ILogger<ResponseService> logger, IConfiguration configuration) : IResponseService
{
    private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

    private readonly HttpClient _httpClient = new();
    private readonly string _yooKassaSecret = $"{configuration[$"YooKassa:Id:{Env}"]!}:{configuration[$"YooKassa:Secret:{Env}"]!}";

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

    public void FillYooKassaHttpClientHeaders()
    {
        _httpClient.DefaultRequestHeaders.Clear();

        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_yooKassaSecret);

        _httpClient.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders
            .Add("Idempotence-Key", Guid.NewGuid().ToString());
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(plainTextBytes));
    }
}