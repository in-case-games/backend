using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces
{
    public interface IPromocodeService
    {
        public Task<List<PromocodeResponse>> GetAsync();
        public Task<List<PromocodeResponse>> GetEmptyPromocodesAsync();
        public Task<PromocodeResponse> GetAsync(string name);
        public Task<List<PromocodeTypeResponse>> GetTypesAsync();
        public Task<PromocodeResponse> CreateAsync(PromocodeRequest request);
        public Task<PromocodeResponse> UpdateAsync(PromocodeRequest request);
        public Task<PromocodeResponse> DeleteAsync(Guid id);
    }
}
