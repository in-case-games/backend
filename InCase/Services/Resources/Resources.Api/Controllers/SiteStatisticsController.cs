using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
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
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SiteStatistics statistics = await context.SiteStatistics
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken) ??
                throw new NotFoundCodeException("Сайт статистика не найдена");

            return ResponseUtil.Ok(statistics);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SiteStatisticsAdmin statistics = await context.SiteStatisticsAdmins
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken) ?? 
                throw new NotFoundCodeException("Админская сайт статистика не найдена");

            return ResponseUtil.Ok(statistics);
        }
    }
}
