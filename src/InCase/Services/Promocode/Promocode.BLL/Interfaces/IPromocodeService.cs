using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces;
public interface IPromoCodeService
{
    public Task<List<PromoCodeResponse>> GetAsync(CancellationToken cancellation = default);
    public Task<List<PromoCodeResponse>> GetEmptyPromoCodesAsync(CancellationToken cancellation = default);
    public Task<PromoCodeResponse> GetAsync(string name, CancellationToken cancellation = default);
    public Task<List<PromoCodeTypeResponse>> GetTypesAsync(CancellationToken cancellation = default);
    public Task<PromoCodeResponse> CreateAsync(PromoCodeRequest request, CancellationToken cancellation = default);
    public Task<PromoCodeResponse> UpdateAsync(PromoCodeRequest request, CancellationToken cancellation = default);
    public Task<PromoCodeResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}