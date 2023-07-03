using Review.BLL.Models;

namespace Review.BLL.Interfaces
{
    public interface IReviewImageService
    {
        public Task<ReviewImageResponse> GetAsync(Guid id, bool isAdmin);
        public Task<List<ReviewImageResponse>> GetAsync(bool isAdmin);
        public Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isAdmin);
        public Task<ReviewImageResponse> CreateAsync(ReviewImageRequest request);
        public Task<ReviewImageResponse> DeleteAsync(Guid id);
    }
}
