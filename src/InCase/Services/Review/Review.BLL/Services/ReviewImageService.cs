using Microsoft.EntityFrameworkCore;
using Review.BLL.Exceptions;
using Review.BLL.Helpers;
using Review.BLL.Interfaces;
using Review.BLL.Models;
using Review.DAL.Data;
using Review.DAL.Entities;

namespace Review.BLL.Services;
public class ReviewImageService(ApplicationDbContext context) : IReviewImageService
{
    public async Task<ReviewImageResponse> GetAsync(Guid id, bool isOnlyApproved, CancellationToken cancellation = default)
    {
        var image = await context.ReviewImages
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
        var images = await context.ReviewImages
            .Include(ri => ri.Review)
            .AsNoTracking()
            .ToListAsync(cancellation);

        if(isOnlyApproved) images = images.Where(ri => ri.Review!.IsApproved).ToList();

        return images.ToResponse();
    }

    public async Task<List<ReviewImageResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved, 
        CancellationToken cancellation = default)
    {
        var images = await context.ReviewImages
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
        var images = await context.ReviewImages
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

        var review = await context.UserReviews
            .FirstOrDefaultAsync(ur => ur.Id == request.ReviewId, cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        var image = new ReviewImage
        {
            Id = Guid.NewGuid(),
            ReviewId = request.ReviewId,
        };

        if (review.UserId != userId) throw new ForbiddenException("Доступ к отзыву только у создателя");

        review.IsApproved = false;

        await context.ReviewImages.AddAsync(image, cancellation);
        await context.SaveChangesAsync(cancellation);

        FileService.UploadImageBase64(request.Image, $"reviews/{image.ReviewId}/{image.Id}/", $"{image.Id}");

        return image.ToResponse();
    }

    public async Task<ReviewImageResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default)
    {
        var image = await context.ReviewImages
            .Include(ri => ri.Review)
            .AsNoTracking()
            .FirstOrDefaultAsync(ri => ri.Id == id, cancellation) ??
            throw new NotFoundException("Изображение не найдено");

        if (image.Review!.UserId != userId) throw new ForbiddenException("Доступ к отзыву только у создателя");

        context.ReviewImages.Remove(image);
        await context.SaveChangesAsync(cancellation);

        FileService.RemoveFolder($"reviews/{image.ReviewId}/{id}/");

        return image.ToResponse();
    }

    public async Task<ReviewImageResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var image = await context.ReviewImages
            .Include(ri => ri.Review)
            .AsNoTracking()
            .FirstOrDefaultAsync(ri => ri.Id == id, cancellation) ??
            throw new NotFoundException("Изображение не найдено");

        context.ReviewImages.Remove(image);
        await context.SaveChangesAsync(cancellation);

        FileService.RemoveFolder($"reviews/{image.ReviewId}/{id}/");

        return image.ToResponse();
    }
}