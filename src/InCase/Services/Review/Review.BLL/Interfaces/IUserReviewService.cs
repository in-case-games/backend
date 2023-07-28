using Review.BLL.Models;

namespace Review.BLL.Interfaces
{
    public interface IUserReviewService
    {
        public Task<UserReviewResponse> GetAsync(Guid id, bool isOnlyApproved);
        public Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved);
        public Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved);
        public Task<UserReviewResponse> ApproveReviewAsync(Guid id);
        public Task<UserReviewResponse> DeniedReviewAsync(Guid id);
        public Task<UserReviewResponse> CreateAsync(UserReviewRequest request);
        public Task<UserReviewResponse> UpdateAsync(UserReviewRequest request);
        public Task<UserReviewResponse> DeleteAsync(Guid userId, Guid id);
        public Task<UserReviewResponse> DeleteAsync(Guid id);
    }
}
