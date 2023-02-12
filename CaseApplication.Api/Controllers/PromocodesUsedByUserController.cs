using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Api.Controllers 
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodesUsedByUserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
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
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetUsedPromocodes(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<PromocodesUsedByUser> promocodesUseds = await context.PromocodeUsedByUsers
                    .AsNoTracking()
                    .Include(x => x.Promocode)
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

            return Ok(promocodesUseds);
        }
    }
}