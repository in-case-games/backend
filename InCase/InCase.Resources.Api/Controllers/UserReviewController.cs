using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
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
                .Where(w => w.IsApproved == true)
                .ToListAsync();

            return ResponseUtil.Ok(reviews);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin()
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

            if (!await context.Users.AnyAsync(a => a.Id == id))
                return NotFound(nameof(User));

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
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserReview> reviews = await context.UserReviews
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(w => w.IsApproved == true)
                .ToListAsync();

            List<ReviewImage> images = new();

            reviews.ForEach(f => images.AddRange(f.Images!));

            return ResponseUtil.Ok(images);
        }

        [AllowAnonymous]
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.UserReviews.AnyAsync(a => a.Id == id && a.IsApproved == true))
                return NotFound(nameof(UserReview));

            List<ReviewImage> images = await context.ReviewImages
                .AsNoTracking()
                .Where(w => w.ReviewId == id)
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

            reviewDto.UserId = UserId;
            reviewDto.IsApproved = false;

            return await EndpointUtil.Create(reviewDto.Convert(), context);
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

            return await EndpointUtil.Create(imageDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(UserReviewDto reviewDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserReview? review = await context.UserReviews
                .FirstOrDefaultAsync(f => 
                f.UserId == UserId && 
                f.Id == reviewDto.Id);

            if (review == null) 
                return ResponseUtil.NotFound(nameof(UserReview));

            reviewDto.IsApproved = review.IsApproved;
            reviewDto.UserId = UserId;

            return await EndpointUtil.Update(review, reviewDto.Convert(false), context);
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

            reviewDto.UserId = review.UserId;

            return await EndpointUtil.Update(review, reviewDto.Convert(false), context);
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

            ReviewImage? image = await context.ReviewImages
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (image is null)
                return ResponseUtil.NotFound(nameof(ReviewImage));

            UserReview? review = await context.UserReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == image.ReviewId);

            if (review is null)
                return ResponseUtil.NotFound(nameof(UserReview));
            if (review!.UserId != UserId)
                return Forbid("Access denied");

            return await EndpointUtil.Delete(image, context);
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
