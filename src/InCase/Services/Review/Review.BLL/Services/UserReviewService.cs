using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;
using Review.BLL.Exceptions;
using Review.BLL.Helpers;
using Review.BLL.Interfaces;
using Review.BLL.MassTransit;
using Review.BLL.Models;
using Review.DAL.Data;

namespace Review.BLL.Services
{
    public class UserReviewService : IUserReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public UserReviewService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<UserReviewResponse> GetAsync(Guid id, bool isOnlyApproved, CancellationToken cancellation = default)
        {
            var review = await _context.Reviews
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
            var reviews = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .ToListAsync(cancellation);

            if(isOnlyApproved) reviews = reviews.Where(ur => ur.IsApproved).ToList();

            return reviews.ToResponse();
        }

        public async Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved, int count, CancellationToken cancellation = default)
        {
            var reviews = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .ToListAsync(cancellation);

            if (isOnlyApproved) reviews = reviews.Where(ur => ur.IsApproved).Take(count).ToList();

            return reviews.ToResponse();
        }

        public async Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved, CancellationToken cancellation = default)
        {
            var reviews = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .ToListAsync(cancellation);

            if (isOnlyApproved) reviews = reviews.Where(ur => ur.IsApproved).ToList();

            return reviews.ToResponse();
        }

        public async Task<UserReviewResponse> CreateAsync(UserReviewRequest request, CancellationToken cancellation = default)
        {
            if (request.Score > 5 || request.Score < 1)
                throw new BadRequestException("Оценка отзыва должна быть больше 1 и меньше 5");
            if (await _context.Reviews.AnyAsync(u => u.UserId == request.UserId, cancellation))
                throw new ConflictException("У вас уже есть отзыв");
            if (!await _context.User.AnyAsync(u => u.Id == request.UserId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            var review = request.ToEntity(isNewGuid: true);

            review.CreationDate = DateTime.UtcNow;
            review.IsApproved = false;

            await _context.Reviews.AddAsync(review, cancellation);
            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(new SiteStatisticsTemplate { Reviews = 1 }, cancellation);

            FileService.CreateFolder($"reviews/{review.Id}/");

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> DeniedReviewAsync(Guid id, CancellationToken cancellation = default)
        {
            var review = await _context.Reviews
                .Include(review => review.Images)
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
                throw new NotFoundException("Отзыв не найден");

            review.IsApproved = false;

            await _context.SaveChangesAsync(cancellation);

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> ApproveReviewAsync(Guid id, CancellationToken cancellation = default)
        {
            var review = await _context.Reviews
                .Include(review => review.Images)
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
                throw new NotFoundException("Отзыв не найден");

            review.IsApproved = true;

            await _context.SaveChangesAsync(cancellation);

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> UpdateAsync(UserReviewRequest request, CancellationToken cancellation = default)
        {
            if (request.Score is > 5 or < 1)
                throw new BadRequestException("Оценка отзыва должна быть больше 1 и меньше 5");
            if (!await _context.User.AnyAsync(u => u.Id == request.UserId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            var review = request.ToEntity(isNewGuid: false);

            var reviewOld = await _context.Reviews
                .Include(ur => ur.Images)
                .FirstOrDefaultAsync(ur => ur.Id == review.Id, cancellationToken: cancellation) ??
                throw new NotFoundException("Отзыв не найден");

            review.CreationDate = reviewOld.CreationDate;
            review.IsApproved = false;
            review.UserId = reviewOld.UserId;

            _context.Entry(reviewOld).CurrentValues.SetValues(review);
            await _context.SaveChangesAsync(cancellation);

            review.Images = reviewOld.Images;

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default)
        {
            var review = await _context.Reviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
                throw new NotFoundException("Отзыв не найден");

            if (review.UserId != userId) throw new ForbiddenException("Доступ к отзыву только у создателя");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(new SiteStatisticsTemplate { Reviews = -1 }, cancellation);

            FileService.RemoveFolder($"reviews/{id}/");

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var review = await _context.Reviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
                throw new NotFoundException("Отзыв не найден");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellation);
            await _publisher.SendAsync(new SiteStatisticsTemplate { Reviews = -1 }, cancellation);

            FileService.RemoveFolder($"reviews/{id}/");

            return review.ToResponse();
        }
    }
}
