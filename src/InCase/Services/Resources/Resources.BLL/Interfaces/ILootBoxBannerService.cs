using Microsoft.AspNetCore.Http;
using Resources.BLL.Entities;
using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxBannerService
    {
        public Task<LootBoxBannerResponse> GetAsync(Guid id);
        public Task<LootBoxBannerResponse> GetByBoxIdAsync(Guid id);
        public Task<List<LootBoxBannerResponse>> GetAsync();
        public Task<LootBoxBannerResponse> CreateAsync(LootBoxBannerRequest request);
        public Task<LootBoxBannerResponse> DeleteAsync(Guid id);
    }
}
