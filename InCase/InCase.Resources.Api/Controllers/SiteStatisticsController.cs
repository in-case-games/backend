using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/site-statistics")]
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
                .FirstAsync();

            if (statistics is null)
                return ResponseUtil.NotFound(nameof(SiteStatistics));

            return ResponseUtil.Ok(statistics);
        }
        [AuthorizeRoles(Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(SiteStatistics statistics)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (await context.SiteStatistics.FindAsync(statistics.Id) is null)
                return ResponseUtil.NotFound(nameof(SiteStatistics));

            try
            {
                context.SiteStatistics.Update(statistics);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(statistics);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
    }
}
