using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces;
public interface IUserPromoCodesService
{
    public Task<UserPromoCodeResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<UserPromoCodeResponse> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default);
    public Task<List<UserPromoCodeResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default);
    public Task<List<UserPromoCodeResponse>> GetAsync(int count, CancellationToken cancellation = default);
    public Task<UserPromoCodeResponse> ActivateAsync(Guid userId, string name, CancellationToken cancellation = default);
    public Task<UserPromoCodeResponse> ExchangeAsync(Guid userId, string name, CancellationToken cancellation = default);
}