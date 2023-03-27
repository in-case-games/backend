using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class SiteStatisticsController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SiteStatisticsController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SiteStatistics? statistics = await context.SiteStatistics
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (statistics == null) 
                return NotFound(new { Error = "Data was not found", Success = false });
            
            statistics.SiteBalance = 0;

            return Ok(new { Data = statistics, Success = true });
        }
    }
}
