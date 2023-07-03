using Review.BLL.Models;

namespace Review.BLL.Interfaces
{
    public interface IUserReviewService
    {
        public Task<UserReviewResponse> GetAsync(Guid id, bool isAdmin);
        public Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isAdmin);
        public Task<List<UserReviewResponse>> GetAsync(bool isAdmin);
        public Task<UserReviewResponse> ApproveReview(Guid id);
        public Task<UserReviewResponse> DeniedReview(Guid id);
        public Task<UserReviewResponse> CreateAsync(UserReviewRequest request);
        public Task<UserReviewResponse> UpdateAsync(UserReviewRequest request);
        public Task<UserReviewResponse> DeleteAsync(Guid id);
    }
}
