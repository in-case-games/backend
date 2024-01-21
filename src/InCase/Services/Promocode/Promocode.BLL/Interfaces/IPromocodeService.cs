using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces;

public interface IPromocodeService
{
    public Task<List<PromocodeResponse>> GetAsync(CancellationToken cancellation = default);
    public Task<List<PromocodeResponse>> GetEmptyPromocodesAsync(CancellationToken cancellation = default);
    public Task<PromocodeResponse> GetAsync(string name, CancellationToken cancellation = default);
    public Task<List<PromocodeTypeResponse>> GetTypesAsync(CancellationToken cancellation = default);
    public Task<PromocodeResponse> CreateAsync(PromocodeRequest request, CancellationToken cancellation = default);
    public Task<PromocodeResponse> UpdateAsync(PromocodeRequest request, CancellationToken cancellation = default);
    public Task<PromocodeResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}