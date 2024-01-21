using Resources.BLL.Models;

namespace Resources.BLL.Interfaces;

public interface IGameService
{
    public Task<List<GameResponse>> GetAsync(CancellationToken cancellation = default);
    public Task<GameResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<GameResponse> GetAsync(string name, CancellationToken cancellation = default);
}