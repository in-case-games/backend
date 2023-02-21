using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class PromocodesUsedByUserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        public PromocodesUsedByUserController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodesUsedByUser? promocodeUsed = await context.PromocodeUsedByUsers
                .AsNoTracking()
                .Include(x => x.Promocode)
                .FirstOrDefaultAsync(x => x.Id == id);

            return promocodeUsed is null ? NotFound() : Ok(promocodeUsed);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetUsedPromocodes()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.PromocodeUsedByUsers
                .AsNoTracking()
                .Include(x => x.Promocode)
                .Where(x => x.UserId == UserId)
                .ToListAsync());
        }
    }
}