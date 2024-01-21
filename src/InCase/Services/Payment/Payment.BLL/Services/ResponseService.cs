using System.Net.Http.Json;
using System.Text.Json;
using Payment.BLL.Interfaces;

namespace Payment.BLL.Services;

public class ResponseService : IResponseService
{
    private readonly HttpClient _httpClient = new();

    public async Task<IGameMoneyResponse?> ResponsePostAsync(string uri, 
        IGameMoneyRequest request, CancellationToken cancellation = default)
    {
        var json = JsonContent.Create(request);
        var response = await _httpClient.PostAsync(uri, json, cancellation);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.StatusCode.ToString() +
                response.RequestMessage! +
                response.Headers +
                response.ReasonPhrase! +
                response.Content);
        }

        return await response.Content.ReadFromJsonAsync<IGameMoneyResponse>(
            new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellation);
    }
}