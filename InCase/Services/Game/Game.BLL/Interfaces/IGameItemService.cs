using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IGameItemService
    {
        public Task<GameItemResponse> GetAsync(Guid id);
        public Task<GameItemResponse> CreateAsync(GameItemRequest request, bool isNewGuid = false);
        public Task<GameItemResponse> UpdateAsync(GameItemRequest request);
        public Task<GameItemResponse> DeleteAsync(Guid id);
    }
}
