using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodeTypeController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public PromocodeTypeController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? promocodeType = await context.PromocodeType
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PromocodeTypeName == name);

            return promocodeType is null ? NotFound(): Ok(promocodeType);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.PromocodeType
                .AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(PromocodeType promocodeType)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            promocodeType.Id = Guid.NewGuid();

            await context.PromocodeType.AddAsync(promocodeType);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(PromocodeType promocodeType)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? promoType = await context.PromocodeType
                .FirstOrDefaultAsync(x => x.Id == promocodeType.Id);

            if (promoType is null)
                return Conflict("PromocodeType, which you search, is not found!");

            context.Entry(promoType).CurrentValues.SetValues(promocodeType);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? promocodeType = await context
                .PromocodeType
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (promocodeType is null)
                return NotFound("PromocodeType, which you search, is not found!");

            context.PromocodeType.Remove(promocodeType);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}