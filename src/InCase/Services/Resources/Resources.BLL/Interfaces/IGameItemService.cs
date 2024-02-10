using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Interfaces;
public interface IGameItemService
{
    public Task<GameItemResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetAsync(CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetAsync(string name, CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetByGameIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetByHashNameAsync(string hash, CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetByTypeAsync(string name, CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetByRarityAsync(string name, CancellationToken cancellation = default);
    public Task<List<GameItemResponse>> GetByQualityAsync(string name, CancellationToken cancellation = default);
    public Task<List<GameItemQuality>> GetQualitiesAsync(CancellationToken cancellation = default);
    public Task<List<GameItemRarity>> GetRaritiesAsync(CancellationToken cancellation = default);
    public Task<List<GameItemType>> GetTypesAsync(CancellationToken cancellation = default);
    public Task<GameItemResponse> CreateAsync(GameItemRequest request, CancellationToken cancellation = default);
    public Task<GameItemResponse> UpdateAsync(GameItemRequest request, CancellationToken cancellation = default);
    public Task<GameItemResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);

    public Task UpdateCostManagerAsync(CancellationToken cancellationToken);
}