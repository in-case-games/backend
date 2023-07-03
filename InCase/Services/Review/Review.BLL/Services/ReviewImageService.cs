using Review.BLL.Interfaces;
using Review.BLL.Models;
using Review.DAL.Data;

namespace Review.BLL.Services
{
    public class ReviewImageService : IReviewImageService
    {
        private readonly ApplicationDbContext _context;

        public ReviewImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ReviewImageResponse> GetAsync(Guid id, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReviewImageResponse>> GetAsync(bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewImageResponse> CreateAsync(ReviewImageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewImageResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
