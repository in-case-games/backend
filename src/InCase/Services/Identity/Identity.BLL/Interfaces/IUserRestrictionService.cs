using Identity.BLL.Models;

namespace Identity.BLL.Interfaces;
public interface IUserRestrictionService
{
    public Task<UserRestrictionResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<UserRestrictionResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default);
    public Task<List<UserRestrictionResponse>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellation = default);
    public Task<List<UserRestrictionResponse>> GetAsync(Guid userId, Guid ownerId, CancellationToken cancellation = default);
    public Task<List<UserRestrictionResponse>> GetByLoginAsync(string login, CancellationToken cancellation = default);
    public Task<List<RestrictionTypeResponse>> GetTypesAsync(CancellationToken cancellation = default);
    public Task<UserRestrictionResponse> CreateAsync(UserRestrictionRequest request, CancellationToken cancellation = default);
    public Task<UserRestrictionResponse> UpdateAsync(UserRestrictionRequest request, CancellationToken cancellation = default);
    public Task<UserRestrictionResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}