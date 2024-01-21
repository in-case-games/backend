using Game.BLL.Models;

namespace Game.BLL.Interfaces;

public interface IUserPathBannerService
{
    public Task<List<UserPathBannerResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default);
    public Task<UserPathBannerResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation = default);
    public Task<UserPathBannerResponse> GetByBoxIdAsync(Guid boxId, Guid userId, CancellationToken cancellation = default);
    public Task<List<UserPathBannerResponse>> GetByItemIdAsync(Guid itemId, Guid userId, CancellationToken cancellation = default);
    public Task<UserPathBannerResponse> CreateAsync(UserPathBannerRequest request, CancellationToken cancellation = default);
    public Task<UserPathBannerResponse> UpdateAsync(UserPathBannerRequest request, CancellationToken cancellation = default);
    public Task<UserPathBannerResponse> DeleteAsync(Guid id, Guid userId, CancellationToken cancellation = default);
}