using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;
using Review.BLL.Exceptions;
using Review.BLL.Helpers;
using Review.BLL.Interfaces;
using Review.BLL.MassTransit;
using Review.BLL.Models;
using Review.DAL.Data;

namespace Review.BLL.Services;
public class UserReviewService(ApplicationDbContext context, BasePublisher publisher) : IUserReviewService
{
    public async Task<UserReviewResponse> GetAsync(Guid id, bool isOnlyApproved, CancellationToken cancellation = default)
    {
        var review = await context.UserReviews
            .Include(review => review.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        return isOnlyApproved is false || review.IsApproved ?
            review.ToResponse() :
            throw new ForbiddenException("Отзыв не одобрен администрацией");
    }

    public async Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved, CancellationToken cancellation = default)
    {
        var reviews = await context.UserReviews
            .Include(review => review.Images)
            .AsNoTracking()
            .ToListAsync(cancellation);

        if(isOnlyApproved) reviews = reviews.Where(ur => ur.IsApproved).ToList();

        return reviews.ToResponse();
    }

    public async Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved, int count, CancellationToken cancellation = default)
    {
        var reviews = await context.UserReviews
            .Include(review => review.Images)
            .AsNoTracking()
            .ToListAsync(cancellation);

        if (isOnlyApproved) reviews = reviews.Where(ur => ur.IsApproved).Take(count).ToList();

        return reviews.ToResponse();
    }

    public async Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved, CancellationToken cancellation = default)
    {
        var reviews = await context.UserReviews
            .Include(review => review.Images)
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .ToListAsync(cancellation);

        if (isOnlyApproved) reviews = reviews.Where(ur => ur.IsApproved).ToList();

        return reviews.ToResponse();
    }

    public async Task<UserReviewResponse> CreateAsync(UserReviewRequest request, CancellationToken cancellation = default)
    {
        ValidationService.IsUserReview(request);

        if (await context.UserReviews.AnyAsync(u => u.UserId == request.UserId, cancellation))
            throw new ConflictException("У вас уже есть отзыв");
        if (!await context.Users.AnyAsync(u => u.Id == request.UserId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var review = request.ToEntity(isNewGuid: true);

        review.CreationDate = DateTime.UtcNow;
        review.IsApproved = false;

        await context.UserReviews.AddAsync(review, cancellation);
        await context.SaveChangesAsync(cancellation);
        await publisher.SendAsync(new SiteStatisticsTemplate { Reviews = 1 }, cancellation);

        FileService.CreateFolder($"reviews/{review.Id}/");

        return review.ToResponse();
    }

    public async Task<UserReviewResponse> DeniedReviewAsync(Guid id, CancellationToken cancellation = default)
    {
        var review = await context.UserReviews
            .Include(review => review.Images)
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        review.IsApproved = false;

        await context.SaveChangesAsync(cancellation);

        return review.ToResponse();
    }

    public async Task<UserReviewResponse> ApproveReviewAsync(Guid id, CancellationToken cancellation = default)
    {
        var review = await context.UserReviews
            .Include(review => review.Images)
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        review.IsApproved = true;

        await context.SaveChangesAsync(cancellation);

        return review.ToResponse();
    }

    public async Task<UserReviewResponse> UpdateAsync(UserReviewRequest request, CancellationToken cancellation = default)
    {
        ValidationService.IsUserReview(request);

        if (!await context.Users.AnyAsync(u => u.Id == request.UserId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var review = request.ToEntity(isNewGuid: false);

        var reviewOld = await context.UserReviews
            .Include(ur => ur.Images)
            .FirstOrDefaultAsync(ur => ur.Id == review.Id, cancellationToken: cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        review.CreationDate = reviewOld.CreationDate;
        review.IsApproved = false;
        review.UserId = reviewOld.UserId;

        context.Entry(reviewOld).CurrentValues.SetValues(review);
        await context.SaveChangesAsync(cancellation);

        review.Images = reviewOld.Images;

        return review.ToResponse();
    }

    public async Task<UserReviewResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default)
    {
        var review = await context.UserReviews
            .Include(ur => ur.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        if (review.UserId != userId) throw new ForbiddenException("Доступ к отзыву только у создателя");

        context.UserReviews.Remove(review);
        await context.SaveChangesAsync(cancellation);
        await publisher.SendAsync(new SiteStatisticsTemplate { Reviews = -1 }, cancellation);

        FileService.RemoveFolder($"reviews/{id}/");

        return review.ToResponse();
    }

    public async Task<UserReviewResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var review = await context.UserReviews
            .Include(ur => ur.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Отзыв не найден");

        context.UserReviews.Remove(review);
        await context.SaveChangesAsync(cancellation);
        await publisher.SendAsync(new SiteStatisticsTemplate { Reviews = -1 }, cancellation);

        FileService.RemoveFolder($"reviews/{id}/");

        return review.ToResponse();
    }
}