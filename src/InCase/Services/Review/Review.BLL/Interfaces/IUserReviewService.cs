using Review.BLL.Models;

namespace Review.BLL.Interfaces
{
    public interface IUserReviewService
    {
        public Task<UserReviewResponse> GetAsync(Guid id, bool isOnlyApproved, CancellationToken cancellation = default);
        public Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved, CancellationToken cancellation = default);
        public Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved, CancellationToken cancellation = default);
        public Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved, int count, CancellationToken cancellation = default);
        public Task<UserReviewResponse> ApproveReviewAsync(Guid id, CancellationToken cancellation = default);
        public Task<UserReviewResponse> DeniedReviewAsync(Guid id, CancellationToken cancellation = default);
        public Task<UserReviewResponse> CreateAsync(UserReviewRequest request, CancellationToken cancellation = default);
        public Task<UserReviewResponse> UpdateAsync(UserReviewRequest request, CancellationToken cancellation = default);
        public Task<UserReviewResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default);
        public Task<UserReviewResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
