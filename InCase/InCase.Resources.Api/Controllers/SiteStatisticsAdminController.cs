using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/site-statistics-admin")]
    [ApiController]
    public class SiteStatisticsAdminController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public SiteStatisticsAdminController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SiteStatisticsAdmin? statistics = await context.SiteStatisticsAdmins
                .AsNoTracking()
                .FirstAsync();

            if (statistics is null)
                return ResponseUtil.NotFound(nameof(SiteStatisticsAdmin));

            return ResponseUtil.Ok(statistics);
        }
    }
}
