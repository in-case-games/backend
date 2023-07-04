using Review.BLL.Models;

namespace Review.BLL.Interfaces
{
    public interface IUserReviewService
    {
        public Task<UserReviewResponse> GetAsync(Guid id, bool isOnlyApproved);
        public Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved);
        public Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved);
        public Task<UserReviewResponse> ApproveReview(Guid id);
        public Task<UserReviewResponse> DeniedReview(Guid id);
        public Task<UserReviewResponse> CreateAsync(UserReviewRequest request);
        public Task<UserReviewResponse> UpdateAsync(Guid userId, UserReviewRequest request);
        public Task<UserReviewResponse> DeleteAsync(Guid userId, Guid id);
    }
}
