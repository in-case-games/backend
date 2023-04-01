using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public NewsController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IEnumerable<News> news = await context.News
                .Include(x => x.Images)
                .ToListAsync();

            return ResponseUtil.Ok(news);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .Include(x => x.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
                return ResponseUtil.NotFound(nameof(News));

            return ResponseUtil.Ok(news);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.News.AddAsync(news);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(news);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpPost("{id}/images")]
        public async Task<IActionResult> CreateImage(Guid id, NewsImageDto newsImage)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                if (await context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id) is null)
                {

                    return ResponseUtil.NotFound(nameof(NewsImage));
                }

                newsImage.NewsId = id;
                await context.NewsImages.AddAsync(newsImage.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(newsImage);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                if (await context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id) is null)
                {

                    return ResponseUtil.NotFound(nameof(News));
                }

                news.Id = id;

                context.News.Update(news);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(news);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
                return ResponseUtil.NotFound(nameof(News));

            context.News.Remove(news);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(news);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner, Roles.Bot)]
        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            NewsImage? image = await context.NewsImages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image is null)
                return ResponseUtil.NotFound(nameof(NewsImage));

            context.NewsImages.Remove(image);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(image);
        }
    }
}
