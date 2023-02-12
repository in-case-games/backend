using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
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
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return news is null ? NotFound(): Ok(news);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.News
                .AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            await context.News.AddAsync(news);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(News news)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? searchNews = await context.News.FirstOrDefaultAsync(x => x.Id == news.Id);

            if (searchNews is null)
                return NotFound("There is no such news in the database, " +
                    "review what data comes from the api");

            context.Entry(searchNews).CurrentValues.SetValues(news);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            News? searchNews = await context.News
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (searchNews is null)
                return NotFound("There is no such news in the database, " +
                    "review what data comes from the api");

            context.Remove(searchNews);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
