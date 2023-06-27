using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IUserOpeningService
    {
        public Task<UserOpeningResponse> GetAsync(Guid id);
        public Task<List<UserOpeningResponse>> GetAsync(int count);
        public Task<List<UserOpeningResponse>> GetAsync(Guid userId, int count);
        public Task<List<UserOpeningResponse>> GetByBoxIdAsync(Guid userId, Guid boxId, int count);
        public Task<List<UserOpeningResponse>> GetByItemIdAsync(Guid userId, Guid itemId, int count);
        public Task<List<UserOpeningResponse>> GetByBoxIdAsync(Guid boxId, int count);
        public Task<List<UserOpeningResponse>> GetByItemIdAsync(Guid itemId, int count);
    }
}
