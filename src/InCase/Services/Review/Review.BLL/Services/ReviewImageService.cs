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

        public async Task<ReviewImageResponse> GetAsync(Guid id, bool isOnlyApproved, CancellationToken cancellation = default)
        {
            var image = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id, cancellation) ??
                throw new NotFoundException("Изображение не найдено");

            return isOnlyApproved is false || image.Review!.IsApproved ? 
                image.ToResponse() : 
                throw new ForbiddenException("Изображение не одобренно администрацией");
        }

        public async Task<List<ReviewImageResponse>> GetAsync(bool isOnlyApproved, CancellationToken cancellation = default)
        {
            var images = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .ToListAsync(cancellation);

            if(isOnlyApproved) images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved, 
            CancellationToken cancellation = default)
        {
            var images = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .Where(ri => ri.Review!.UserId == userId)
                .ToListAsync(cancellation);

            if (isOnlyApproved) images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<List<ReviewImageResponse>> GetByReviewIdAsync(Guid reviewId, bool isOnlyApproved, 
            CancellationToken cancellation = default)
        {
            var images = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .Where(ri => ri.ReviewId == reviewId)
                .ToListAsync(cancellation);

            if (isOnlyApproved) images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<ReviewImageResponse> CreateAsync(Guid userId, ReviewImageRequest request, 
            CancellationToken cancellation = default)
        {
            if (request.Image is null) throw new BadRequestException("Загрузите картинку в base 64");

            var review = await _context.Reviews
                .FirstOrDefaultAsync(ur => ur.Id == request.ReviewId, cancellation) ??
                throw new NotFoundException("Отзыв не найден");

            var image = new ReviewImage()
            {
                Id = Guid.NewGuid(),
                ReviewId = request.ReviewId,
            };

            if (review.UserId != userId) throw new ForbiddenException("Доступ к отзыву только у создателя");

            review.IsApproved = false;
            
            FileService.UploadImageBase64(request.Image, @$"reviews/{image.ReviewId}/{image.Id}/", $"{image.Id}");

            await _context.Images.AddAsync(image, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return image.ToResponse();
        }

        public async Task<ReviewImageResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default)
        {
            var image = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id, cancellation) ??
                throw new NotFoundException("Изображение не найдено");

            if (image.Review!.UserId != userId) throw new ForbiddenException("Доступ к отзыву только у создателя");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync(cancellation);

            FileService.RemoveFolder(@$"reviews/{image.ReviewId}/{id}/");

            return image.ToResponse();
        }

        public async Task<ReviewImageResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var image = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id, cancellation) ??
                throw new NotFoundException("Изображение не найдено");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync(cancellation);

            FileService.RemoveFolder(@$"reviews/{image.ReviewId}/{id}/");

            return image.ToResponse();
        }
    }
}
