using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                .Include(i => i.Images)
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return review is null ? 
                ResponseUtil.NotFound(nameof(UserReview)) : 
                ResponseUtil.Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview> reviews = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(w => w.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview>? reviews = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AllowAnonymous]
        [HttpGet("images")]
        public async Task<IActionResult> GetImages()
        {
            return await EndpointUtil.GetAll<ReviewImage>(_context);
        }

        [AllowAnonymous]
        [HttpGet("{reviewId}/images")]
        public async Task<IActionResult> GetImages(Guid reviewId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<ReviewImage> images = await context.ReviewImages
                .AsNoTracking()
                .Where(w => w.ReviewId == reviewId)
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

            if (reviewDto.UserId != UserId)
                return Forbid("Access denied");

            try
            {
                await context.UserReviews.AddAsync(reviewDto.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(reviewDto);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost("image")]
        public async Task<IActionResult> CreateImage(ReviewImageDto imageDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? userReview = await context.UserReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == imageDto.ReviewId);

            if (userReview is null)
                return ResponseUtil.NotFound(nameof(UserReview));
            if (userReview.UserId != UserId)
                return Forbid("Access denied");

            try
            {
                await context.ReviewImages.AddAsync(imageDto.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(imageDto);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (reviewDto.UserId != UserId)
                return Forbid("Access denied");

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(f => 
                f.UserId == reviewDto.UserId && 
                f.Id == reviewDto.Id);

            if (review == null) 
                return ResponseUtil.NotFound(nameof(UserReview));

            try
            {
                context.Entry(review).CurrentValues.SetValues(reviewDto.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(reviewDto);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateByAdmin(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(f => f.Id == reviewDto.Id);

            if (review == null)
                return ResponseUtil.NotFound(nameof(UserReview));

            try
            {
                context.Entry(review).CurrentValues.SetValues(reviewDto.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(reviewDto);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(f => f.Id == id);

            if (review == null)
                return ResponseUtil.NotFound(nameof(UserReview));
            if (review.UserId != UserId)
                return Forbid("Access denied");

            try
            {
                context.UserReviews.Remove(review);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(review);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(f => f.Id == id);

            if (review == null)
                return ResponseUtil.NotFound(nameof(UserReview));

            try
            {
                context.UserReviews.Remove(review);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(review);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("{id}/image/{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (review is null)
                return ResponseUtil.NotFound(nameof(UserReview));
            if (review!.UserId != UserId)
                return Forbid("Access denied");

            ReviewImage? image = review.Images?.FirstOrDefault(x => x.Id == imageId);

            if (image is null)
                return ResponseUtil.NotFound(nameof(ReviewImage));

            try
            {
                context.ReviewImages.Remove(image);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(image);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("admin/image/{id}")]
        public async Task<IActionResult> DeleteAdminImage(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            ReviewImage? reviewImage = await context.ReviewImages
                .FirstOrDefaultAsync(f => f.Id == id);

            if (reviewImage == null)
                return ResponseUtil.NotFound(nameof(UserReview));

            try
            {
                context.ReviewImages.Remove(reviewImage);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(reviewImage);
        }
    }
}
