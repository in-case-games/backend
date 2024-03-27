using Resources.BLL.Models;

namespace Resources.BLL.Interfaces;
public interface IGamePlatformService
{
    public Task<ItemCostResponse> GetOriginalMarketAsync(string hashName, string game, CancellationToken cancellation = default);
    public Task<ItemCostResponse> GetAdditionalMarketAsync(string idForMarket, string game, CancellationToken cancellation = default);
}