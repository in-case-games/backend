using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces;

public interface IUserPromocodesService
{
    public Task<UserPromocodeResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<UserPromocodeResponse> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default);
    public Task<List<UserPromocodeResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default);
    public Task<List<UserPromocodeResponse>> GetAsync(int count, CancellationToken cancellation = default);
    public Task<UserPromocodeResponse> ActivateAsync(Guid userId, string name, CancellationToken cancellation = default);
    public Task<UserPromocodeResponse> ExchangeAsync(Guid userId, string name, CancellationToken cancellation = default);
}