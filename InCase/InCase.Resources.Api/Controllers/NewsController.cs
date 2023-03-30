using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/[controller]")]
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
    }
}
