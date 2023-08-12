using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
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
            ReviewImage image = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id) ??
                throw new NotFoundException("Изображение не найдено");

            return isOnlyApproved is false || image.Review!.IsApproved ? 
                image.ToResponse() : 
                throw new ForbiddenException("Изображение не одобренно администрацией");
        }

        public async Task<List<ReviewImageResponse>> GetAsync(bool isOnlyApproved)
        {
            List<ReviewImage> images = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .ToListAsync();

            if(isOnlyApproved)
                images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved)
        {
            List<ReviewImage> images = await _context.Images
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
            List<ReviewImage> images = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .Where(ri => ri.ReviewId == reviewId)
                .ToListAsync();

            if (isOnlyApproved)
                images = images.Where(ri => ri.Review!.IsApproved).ToList();

            return images.ToResponse();
        }

        public async Task<ReviewImageResponse> CreateAsync(Guid userId, ReviewImageRequest request, IFormFile uploadImage)
        {
            UserReview review = await _context.Reviews
                .FirstOrDefaultAsync(ur => ur.Id == request.ReviewId) ??
                throw new NotFoundException("Отзыв не найден");

            ReviewImage image = request.ToEntity(IsNewGuid: true);

            if (review.UserId != userId)
                throw new ForbiddenException("Доступ к отзыву только у создателя");

            review.IsApproved = false;

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            FileService.Upload(uploadImage, 
                path + $"\\src\\fileserver_imitation\\reviews\\{image.ReviewId}\\{image.Id}\\" + image.Id + ".jpg");

            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }

        public async Task<ReviewImageResponse> DeleteAsync(Guid userId, Guid id)
        {
            ReviewImage image = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id) ??
                throw new NotFoundException("Изображение не найдено");

            if (image.Review!.UserId != userId)
                throw new ForbiddenException("Доступ к отзыву только у создателя");

            // Temp fileserver imitation

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            File.Delete(path + $"src\\fileserver_imitation\\reviews\\{image.ReviewId}\\{image.Id}\\" + image.Id + ".jpg");
            FileService.RemoveFolder(path + $"\\src\\fileserver_imitation\\reviews\\{image.ReviewId}\\{image.Id}");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }

        public async Task<ReviewImageResponse> DeleteAsync(Guid id)
        {
            ReviewImage image = await _context.Images
                .Include(ri => ri.Review)
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id) ??
                throw new NotFoundException("Изображение не найдено");

            // Temp fileserver imitation

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            File.Delete(path + $"src\\fileserver_imitation\\reviews\\{image.ReviewId}\\{image.Id}\\" + image.Id + ".jpg");
            FileService.RemoveFolder(path + $"\\src\\fileserver_imitation\\reviews\\{image.ReviewId}\\{image.Id}");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }
    }
}
