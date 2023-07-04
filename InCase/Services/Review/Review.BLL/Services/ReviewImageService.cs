using Microsoft.EntityFrameworkCore;
using Review.BLL.Exceptions;
using Review.BLL.Helpers;
using Review.BLL.Interfaces;
using Review.BLL.Models;
using Review.DAL.Data;
using Review.DAL.Entities;

namespace Review.BLL.Services
{
    public class ReviewImageService : IReviewImageService
    {
        private readonly ApplicationDbContext _context;

        public ReviewImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewImageResponse> GetAsync(Guid id, bool isOnlyApproved)
        {
            ReviewImage image = await _context.ReviewImages
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id) ??
                throw new NotFoundException("Изображение не найдено");

            if (isOnlyApproved && image.Review!.IsApproved is false)
                throw new ForbiddenException("Изображение не одобренно администрацией");

            return image.ToResponse();
        }

        public async Task<List<ReviewImageResponse>> GetAsync(bool isOnlyApproved)
        {
            List<ReviewImage> images = await _context.ReviewImages
                .Include(ri => ri.Review)
                .AsNoTracking()
                .ToListAsync();

            if(isOnlyApproved)
                images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved)
        {
            List<ReviewImage> images = await _context.ReviewImages
                .Include(ri => ri.Review)
                .AsNoTracking()
                .Where(ri => ri.Review!.UserId == userId)
                .ToListAsync();

            if (isOnlyApproved)
                images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<List<ReviewImageResponse>> GetByReviewIdAsync(Guid reviewId, bool isOnlyApproved)
        {
            List<ReviewImage> images = await _context.ReviewImages
                .Include(ri => ri.Review)
                .AsNoTracking()
                .Where(ri => ri.ReviewId == reviewId)
                .ToListAsync();

            if (isOnlyApproved)
                images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<ReviewImageResponse> CreateAsync(Guid userId, ReviewImageRequest request)
        {
            UserReview review = await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == request.ReviewId) ??
                throw new NotFoundException("Отзыв не найден");

            if (review.UserId != userId)
                throw new ForbiddenException("Доступ к отзыву только у создателя");

            ReviewImage image = request.ToEntity(IsNewGuid: true);

            //TODO Save image local folder 

            await _context.ReviewImages.AddAsync(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }

        public async Task<ReviewImageResponse> DeleteAsync(Guid userId, Guid id)
        {
            ReviewImage image = await _context.ReviewImages
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id) ??
                throw new NotFoundException("Изображение не найдено");

            if (image.Review!.UserId != userId)
                throw new ForbiddenException("Доступ к отзыву только у создателя");

            //TODO Remove image local folder 

            _context.ReviewImages.Remove(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }
    }
}
