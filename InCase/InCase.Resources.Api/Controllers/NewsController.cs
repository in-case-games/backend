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

            List<News> news = await context.News
                .Include(i => i.Images)
                .ToListAsync();

            return ResponseUtil.Ok(news);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return news is null ? 
                ResponseUtil.NotFound(nameof(News)) : 
                ResponseUtil.Ok(news);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            news.Id = Guid.NewGuid();

            return await EndpointUtil.Create(news, _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost("image")]
        public async Task<IActionResult> CreateImage(NewsImageDto imageDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                if (await context.News.AnyAsync(a => a.Id == imageDto.NewsId))
                {
                    await context.NewsImages.AddAsync(imageDto.Convert());
                    await context.SaveChangesAsync();

                    return ResponseUtil.Ok(imageDto.Convert());
                }

                return ResponseUtil.NotFound(nameof(NewsImage));
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(News news)
        {
            return await EndpointUtil.Update(news, _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<News>(id, _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("image/{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            return await EndpointUtil.Delete<NewsImage>(id, _contextFactory);
        }
    }
}
