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
                .AsNoTracking()
                .ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(PromocodeType type)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            type.Id = Guid.NewGuid();

            await context.PromocodeType.AddAsync(type);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(PromocodeType newType)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? oldType = await context.PromocodeType
                .FirstOrDefaultAsync(x => x.Id == newType.Id);

            if (oldType is null) return NotFound();

            context.Entry(oldType).CurrentValues.SetValues(newType);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? type = await context.PromocodeType
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (type is null)return NotFound();

            context.PromocodeType.Remove(type);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}