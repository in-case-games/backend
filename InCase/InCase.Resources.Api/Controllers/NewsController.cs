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

            return news.Count == 0 ? 
                ResponseUtil.NotFound(nameof(News)) : 
                ResponseUtil.Ok(news);
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

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            news.Id = Guid.NewGuid();

            return await EndpointUtil.Create(news, context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost("image")]
        public async Task<IActionResult> CreateImage(NewsImageDto imageDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.News.AnyAsync(a => a.Id == imageDto.NewsId))
                return ResponseUtil.NotFound(nameof(NewsImage));

            return await EndpointUtil.Create(imageDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Update(news, context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<News>(id, context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("image/{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<NewsImage>(id, context);
        }
    }
}
