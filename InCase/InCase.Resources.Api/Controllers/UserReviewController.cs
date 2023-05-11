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
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.IsApproved)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview review = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundCodeException("Отзыв не найден");

            return ResponseUtil.Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(u => u.Id == id))
                throw new NotFoundCodeException("Пользователь не найден");

            List<UserReview> reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview>? reviews = await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        //TODO
        [AllowAnonymous]
        [HttpGet("images")]
        public async Task<IActionResult> GetImages()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<ReviewImage> images = new();

            await context.UserReviews
                .Include(ur => ur.Images)
                .AsNoTracking()
                .Where(ur => ur.IsApproved)
                .ForEachAsync(f => images.AddRange(f.Images!));

            return ResponseUtil.Ok(images);
        }

        [AllowAnonymous]
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if(!await context.UserReviews.AnyAsync(ur => ur.Id == id && ur.IsApproved))
                throw new NotFoundCodeException("Отзыв не найден или не одобрен");

            List<ReviewImage> images = await context.ReviewImages
                .AsNoTracking()
                .Where(ri => ri.ReviewId == id)
                .ToListAsync();

            return ResponseUtil.Ok(images);
        }

        [AllowAnonymous]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            return await EndpointUtil.GetById<ReviewImage>(id, _context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (reviewDto.Score < 0 || reviewDto.Score > 10)
                throw new BadRequestCodeException("Оценка отзыва не может быть быть меньше 0 и больше 10");

            reviewDto.UserId = UserId;
            reviewDto.IsApproved = false;
            reviewDto.CreationDate = DateTime.UtcNow;

            return await EndpointUtil.Create(reviewDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost("image")]
        public async Task<IActionResult> CreateImage(ReviewImageDto imageDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview review = await context.UserReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == imageDto.ReviewId) ?? 
                throw new NotFoundCodeException("Отзыв не найден");

            if (review.UserId != UserId)
                throw new ForbiddenCodeException("Доступ к отзыву только у создателя");

            return await EndpointUtil.Create(imageDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview review = await context.UserReviews
                .FirstOrDefaultAsync(ur => ur.UserId == UserId && ur.Id == reviewDto.Id) ??
                throw new NotFoundCodeException("Отзыв не найден");

            if (reviewDto.Score < 0 || reviewDto.Score > 10)
                throw new BadRequestCodeException("Оценка отзыва не может быть быть меньше 0 и больше 10");

            reviewDto.IsApproved = review.IsApproved;
            reviewDto.UserId = UserId;
            reviewDto.CreationDate = review.CreationDate;

            return await EndpointUtil.Update(review, reviewDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateByAdmin(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview review = await context.UserReviews
                .FirstOrDefaultAsync(ur => ur.Id == reviewDto.Id) ??
                throw new NotFoundCodeException("Отзыв не найден");

            if (reviewDto.Score < 0 || reviewDto.Score > 10)
                throw new BadRequestCodeException("Оценка отзыва не может быть быть меньше 0 и больше 10");

            reviewDto.UserId = review.UserId;

            return await EndpointUtil.Update(review, reviewDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview review = await context.UserReviews
                .FirstOrDefaultAsync(ur => ur.Id == id) ?? 
                throw new NotFoundCodeException("Отзыв не найден");

            if (review.UserId != UserId)
                throw new ForbiddenCodeException("Доступ к отзыву только у создателя");

            return await EndpointUtil.Delete(review, context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            return await EndpointUtil.Delete<UserReview>(id, context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("image/{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            ReviewImage image = await context.ReviewImages
                .AsNoTracking()
                .FirstOrDefaultAsync(ri => ri.Id == id) ??
                throw new NotFoundCodeException("Картинка не найдена");

            UserReview? review = await context.UserReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == image.ReviewId);

            return review!.UserId != UserId ?
                throw new ForbiddenCodeException("Доступ к отзыву только у создателя") : 
                await EndpointUtil.Delete(image, context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("admin/image/{id}")]
        public async Task<IActionResult> DeleteAdminImage(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            return await EndpointUtil.Delete<ReviewImage>(id, context);
        }
    }
}
