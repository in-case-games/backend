using Microsoft.AspNetCore.Http;
using Review.BLL.Models;

namespace Review.BLL.Interfaces
{
    public interface IReviewImageService
    {
        public Task<ReviewImageResponse> GetAsync(Guid id, bool isOnlyApproved);
        public Task<List<ReviewImageResponse>> GetAsync(bool isOnlyApproved);
        public Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved);
        public Task<List<ReviewImageResponse>> GetByReviewIdAsync(Guid reviewId, bool isOnlyApproved);
        public Task<ReviewImageResponse> CreateAsync(Guid userId, ReviewImageRequest request, IFormFile uploadImage);
        public Task<ReviewImageResponse> DeleteAsync(Guid userId, Guid id);
        public Task<ReviewImageResponse> DeleteAsync(Guid id);
    }
}
