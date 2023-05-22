using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface ILootBoxService
    {
        public Task<LootBoxResponse> Get(Guid id);
        public Task<List<LootBoxResponse>> Get();
        public Task<List<LootBoxResponse>> GetByGameId(Guid id);
        public Task<LootBoxResponse> Create(LootBoxRequest request);
        public Task<LootBoxResponse> Update(LootBoxRequest request);
        public Task<LootBoxResponse> Delete(Guid id);
    }
}
