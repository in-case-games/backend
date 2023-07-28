﻿using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;
using Review.BLL.Exceptions;
using Review.BLL.Helpers;
using Review.BLL.Interfaces;
using Review.BLL.MassTransit;
using Review.BLL.Models;
using Review.DAL.Data;
using Review.DAL.Entities;

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

        public async Task<UserReviewResponse> GetAsync(Guid id, bool isOnlyApproved)
        {
            UserReview review = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Отзыв не найден");

            return isOnlyApproved is false || review.IsApproved ?
                review.ToResponse() :
                throw new ForbiddenException("Отзыв не одобрен администрацией");
        }

        public async Task<List<UserReviewResponse>> GetAsync(bool isOnlyApproved)
        {
            List<UserReview> reviews = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .ToListAsync();

            if(isOnlyApproved)
                reviews = reviews.Where(ur => ur.IsApproved).ToList();

            return reviews.ToResponse();
        }

        public async Task<List<UserReviewResponse>> GetByUserIdAsync(Guid userId, bool isOnlyApproved)
        {
            List<UserReview> reviews = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            if (isOnlyApproved)
                reviews = reviews.Where(ur => ur.IsApproved).ToList();

            return reviews.ToResponse();
        }

        public async Task<UserReviewResponse> CreateAsync(UserReviewRequest request)
        {
            if (!await _context.User.AnyAsync(u => u.Id == request.UserId))
                throw new NotFoundException("Пользователь не найден");

            UserReview review = request.ToEntity(isNewGuid: true);
            SiteStatisticsTemplate template = new() { Reviews = 1 };

            review.IsApproved = false;

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(template, "/statistics");

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> DeniedReviewAsync(Guid id)
        {
            UserReview review = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Отзыв не найден");

            review.IsApproved = false;

            await _context.SaveChangesAsync();

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> ApproveReviewAsync(Guid id)
        {
            UserReview review = await _context.Reviews
                .Include(review => review.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Отзыв не найден");

            review.IsApproved = true;

            await _context.SaveChangesAsync();

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> UpdateAsync(UserReviewRequest request)
        {
            if (!await _context.User.AnyAsync(u => u.Id == request.UserId))
                throw new NotFoundException("Пользователь не найден");

            UserReview review = request.ToEntity(isNewGuid: false);

            UserReview reviewOld = await _context.Reviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == review.Id) ??
                throw new NotFoundException("Отзыв не найден");

            review.IsApproved = false;

            _context.Entry(reviewOld).CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();

            review.Images = reviewOld.Images;

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> DeleteAsync(Guid userId, Guid id)
        {
            UserReview review = await _context.Reviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Отзыв не найден");

            SiteStatisticsTemplate template = new() { Reviews = -1 };

            if (review.UserId != userId)
                throw new ForbiddenException("Доступ к отзыву только у создателя");

            //TODO Remove image local folder 

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(template, "/statistics");

            return review.ToResponse();
        }

        public async Task<UserReviewResponse> DeleteAsync(Guid id)
        {
            UserReview review = await _context.Reviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Отзыв не найден");

            SiteStatisticsTemplate template = new() { Reviews = -1 };

            //TODO Remove image local folder 

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(template, "/statistics");

            return review.ToResponse();
        }
    }
}