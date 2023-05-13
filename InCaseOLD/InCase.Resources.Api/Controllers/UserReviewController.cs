using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/user-review")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserReviewController(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.IsApproved)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(reviews);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserReview review = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Отзыв не найден");

            return ResponseUtil.Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if (!await context.Users.AnyAsync(u => u.Id == id, cancellationToken))
                throw new NotFoundCodeException("Пользователь не найден");

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.UserId == id)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.UserId == UserId)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(reviews);
        }

        //TODO
        [AllowAnonymous]
        [HttpGet("images")]
        public async Task<IActionResult> GetImages(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            List<ReviewImage> images = new();

            await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.IsApproved)
                .ForEachAsync(f => images.AddRange(f.Images!), cancellationToken);

            return ResponseUtil.Ok(images);
        }

        [AllowAnonymous]
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImages(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if(!await context.UserReviews.AnyAsync(ur => ur.Id == id && ur.IsApproved, cancellationToken))
                throw new NotFoundCodeException("Отзыв не найден или не одобрен");

            List<ReviewImage> images = await context.ReviewImages
                .AsNoTracking()
                .Where(ri => ri.ReviewId == id)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(images);
        }

        [AllowAnonymous]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(Guid id, CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetById<ReviewImage>(id, _context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(UserReviewDto reviewDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            if (reviewDto.Score < 0 || reviewDto.Score > 10)
                throw new BadRequestCodeException("Оценка отзыва не может быть быть меньше 0 и больше 10");

            reviewDto.UserId = UserId;
            reviewDto.IsApproved = false;
            reviewDto.CreationDate = DateTime.UtcNow;

            return await EndpointUtil.Create(reviewDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost("image")]
        public async Task<IActionResult> CreateImage(ReviewImageDto imageDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserReview review = await context.UserReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == imageDto.ReviewId, cancellationToken) ?? 
                throw new NotFoundCodeException("Отзыв не найден");

            if (review.UserId != UserId)
                throw new ForbiddenCodeException("Доступ к отзыву только у создателя");

            return await EndpointUtil.Create(imageDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(UserReviewDto reviewDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserReview review = await context.UserReviews
                .FirstOrDefaultAsync(ur => ur.UserId == UserId && ur.Id == reviewDto.Id, cancellationToken) ??
                throw new NotFoundCodeException("Отзыв не найден");

            if (reviewDto.Score < 0 || reviewDto.Score > 10)
                throw new BadRequestCodeException("Оценка отзыва не может быть быть меньше 0 и больше 10");

            reviewDto.IsApproved = review.IsApproved;
            reviewDto.UserId = UserId;
            reviewDto.CreationDate = review.CreationDate;

            return await EndpointUtil.Update(review, reviewDto.Convert(false), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateByAdmin(UserReviewDto reviewDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserReview review = await context.UserReviews
                .FirstOrDefaultAsync(ur => ur.Id == reviewDto.Id, cancellationToken) ??
                throw new NotFoundCodeException("Отзыв не найден");

            if (reviewDto.Score < 0 || reviewDto.Score > 10)
                throw new BadRequestCodeException("Оценка отзыва не может быть быть меньше 0 и больше 10");

            reviewDto.UserId = review.UserId;

            return await EndpointUtil.Update(review, reviewDto.Convert(false), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            UserReview review = await context.UserReviews
                .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken) ?? 
                throw new NotFoundCodeException("Отзыв не найден");

            if (review.UserId != UserId)
                throw new ForbiddenCodeException("Доступ к отзыву только у создателя");

            return await EndpointUtil.Delete(review, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteByAdmin(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<UserReview>(id, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("image/{id}")]
        public async Task<IActionResult> DeleteImage(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            ReviewImage image = await context.ReviewImages
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Картинка не найдена");

            UserReview? review = await context.UserReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == image.ReviewId, cancellationToken);

            return review!.UserId != UserId ?
                throw new ForbiddenCodeException("Доступ к отзыву только у создателя") : 
                await EndpointUtil.Delete(image, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("admin/image/{id}")]
        public async Task<IActionResult> DeleteAdminImage(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<ReviewImage>(id, context, cancellationToken);
        }
    }
}
