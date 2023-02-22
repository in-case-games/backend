using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public NewsController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return news is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = news, Success = true });
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(new
            {
                Data = await context.News
                .AsNoTracking()
                .ToListAsync(),
                Success = true
            });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            news.Id = new Guid();

            await context.News.AddAsync(news);
            await context.SaveChangesAsync();

            return Ok(new { Message = "News succesfully created", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? oldNews = await context.News.FirstOrDefaultAsync(x => x.Id == news.Id);

            if (oldNews is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.Entry(oldNews).CurrentValues.SetValues(news);
            await context.SaveChangesAsync();

            return Ok(new { Message = "News succesfully updated", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? news = await context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
                return NotFound(new { Error = "Data was not found", Success = false });

            context.Remove(news);
            await context.SaveChangesAsync();

            return Ok(new { Message = "News succesfully deleted", Success = true });
        }
    }
}
