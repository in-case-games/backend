using Microsoft.AspNetCore.Http;
using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxService
    {
        public Task<LootBoxResponse> GetAsync(Guid id);
        public Task<LootBoxResponse> GetAsync(string name);
        public Task<List<LootBoxResponse>> GetAsync();
        public Task<List<LootBoxResponse>> GetByGameIdAsync(Guid id);
        public Task<LootBoxResponse> CreateAsync(LootBoxRequest request);
        public Task<LootBoxResponse> UpdateAsync(LootBoxRequest request);
        public Task<LootBoxResponse> DeleteAsync(Guid id);
    }
}
