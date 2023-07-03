using Review.BLL.Interfaces;
using Review.BLL.Models;
using Review.DAL.Data;

namespace Review.BLL.Services
{
    public class UserReviewService : IUserReviewService
    {
        private readonly ApplicationDbContext _context;

        public UserReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<UserReviewResponse> GetAsync(Guid id, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserReviewResponse>> GetAsync(bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<UserReviewResponse> CreateAsync(UserReviewRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserReviewResponse> DeniedReview(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserReviewResponse> ApproveReview(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserReviewResponse> UpdateAsync(UserReviewRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserReviewResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
