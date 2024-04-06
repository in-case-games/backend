using Game.BLL.Models;

namespace Game.BLL.Interfaces;
public interface ILootBoxOpeningService
{
    public Task<GameItemResponse> OpenBoxAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<GameItemResponse> OpenVirtualBoxAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<List<GameItemBigOpenResponse>> OpenVirtualBoxAsync(Guid userId, Guid id, int count, bool isAdmin = false, CancellationToken cancellation = default);
}