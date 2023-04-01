using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IEnumerable<News> news = await context.News
                .Include(x => x.Images == null? null: x.Images.First())
                .ToListAsync();

            return Ok(new { Data = news, Success = true });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .Include(x => x.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
                return NotFound(new { Data = "Object is not found", Success = false });

            return Ok(new { Data = news, Success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.News.AddAsync(news);
                await context.SaveChangesAsync();

                return Ok(new { Data = news, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false });
            }
        }
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

                    return NotFound(new { Data = "Object (news) is not found [parameter: id]", Success = false });
                }

                newsImage.NewsId = id;
                await context.NewsImages.AddAsync(newsImage.Convert());
                await context.SaveChangesAsync();

                return Ok(new { Data = newsImage, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false });
            }
        }
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

                    return NotFound(new { Data = "Object (news) is not found [parameter: id]", Success = false });
                }

                news.Id = id;

                context.News.Update(news);
                await context.SaveChangesAsync();

                return Ok(new { Data = news, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
                return NotFound(new { Data = "Object is not found", Success = false });

            context.News.Remove(news);
            await context.SaveChangesAsync();

            return Ok(new { Data = "Object was successfully removed", Success = true });
        }
        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            NewsImage? image = await context.NewsImages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image is null)
                return NotFound(new { Data = "Object is not found", Success = false });

            context.NewsImages.Remove(image);
            await context.SaveChangesAsync();

            return Ok(new { Data = "Object was successfully removed", Success = true });
        }
    }
}
