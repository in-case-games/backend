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
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return ResponseUtil.Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview>? reviews = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview>? reviews = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AllowAnonymous]
        [HttpGet("image")]
        public async Task<IActionResult> GetImages()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<ReviewImage> images = await context.ReviewImages
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(images);
        }

        [AllowAnonymous]
        [HttpGet("{reviewId}/image")]
        public async Task<IActionResult> GetImagesByReviewId(Guid reviewId)
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
        public async Task<IActionResult> GetImageById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            ReviewImage? image = await context.ReviewImages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return ResponseUtil.Ok(image);
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
                .FirstOrDefaultAsync(x => x.Id == imageDto.ReviewId);

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
                .FirstOrDefaultAsync(x => 
                x.UserId == reviewDto.UserId && 
                x.Id == reviewDto.Id);

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

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateByAdmin(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(x => x.Id == reviewDto.Id);

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

        [AuthorizeRoles(Roles.All)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(x => x.Id == id);

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

        [AuthorizeRoles(Roles.All)]
        [HttpDelete("{id}/image/{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .Include(x => x.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if(review is null)
                return ResponseUtil.NotFound(nameof(UserReview));
            if (review!.UserId != UserId)
                return Forbid("Access denied");

            ReviewImage? image = review.Images?.FirstOrDefault(x => x.Id == imageId);

            if(image is null)
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

        [AuthorizeRoles(Roles.All)]
        [HttpDelete("admin/image/{id}")]
        public async Task<IActionResult> DeleteAdminImage(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            ReviewImage? reviewImage = await context.ReviewImages
                .FirstOrDefaultAsync(x => x.Id == id);

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

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(x => x.Id == id);

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
    }
}
