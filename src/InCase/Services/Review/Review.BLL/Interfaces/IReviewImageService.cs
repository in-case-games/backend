using Review.BLL.Models;

namespace Review.BLL.Interfaces;
public interface IReviewImageService
{
    public Task<ReviewImageResponse> GetAsync(Guid id, bool isOnlyApproved, CancellationToken cancellation = default);
    public Task<List<ReviewImageResponse>> GetAsync(bool isOnlyApproved, CancellationToken cancellation = default);
    public Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved, CancellationToken cancellation = default);
    public Task<List<ReviewImageResponse>> GetByReviewIdAsync(Guid reviewId, bool isOnlyApproved, CancellationToken cancellation = default);
    public Task<ReviewImageResponse> CreateAsync(Guid userId, ReviewImageRequest request, CancellationToken cancellation = default);
    public Task<ReviewImageResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<ReviewImageResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}