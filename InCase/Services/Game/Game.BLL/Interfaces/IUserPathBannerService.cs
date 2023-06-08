using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IUserPathBannerService
    {
        public Task<List<UserPathBannerResponse>> GetByUserIdAsync(Guid userId);
        public Task<UserPathBannerResponse> GetByIdAsync(Guid id);
        public Task<UserPathBannerResponse> GetByBoxIdAsync(Guid boxId);
        public Task<List<UserPathBannerResponse>> GetByItemIdAsync(Guid itemId);
        public Task<UserPathBannerResponse> CreateAsync(UserPathBannerRequest request);
        public Task<UserPathBannerResponse> UpdateAsync(UserPathBannerRequest request);
        public Task<UserPathBannerResponse> DeleteAsync(Guid id, Guid userId);
    }
}
