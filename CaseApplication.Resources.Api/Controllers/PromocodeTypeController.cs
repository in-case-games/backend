using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
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

            PromocodeType? type = await context.PromocodeType
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PromocodeTypeName == name);

            return type is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = type, Success = true });
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(new
            {
                Data = await context.PromocodeType
                .AsNoTracking()
                .ToListAsync(),
                Success = true
            });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(PromocodeType type)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            type.Id = Guid.NewGuid();

            await context.PromocodeType.AddAsync(type);
            await context.SaveChangesAsync();

            return Ok(new { Message = "PromocodeType succesfully created", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(PromocodeType newType)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? oldType = await context.PromocodeType
                .FirstOrDefaultAsync(x => x.Id == newType.Id);

            if (oldType is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.Entry(oldType).CurrentValues.SetValues(newType);
            await context.SaveChangesAsync();

            return Ok(new { Message = "PromocodeType succesfully updated", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? type = await context.PromocodeType
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (type is null)
                return NotFound(new { Error = "Data was not found", Success = false });

            context.PromocodeType.Remove(type);
            await context.SaveChangesAsync();

            return Ok(new { Message = "PromocodeType succesfully deleted", Success = true });
        }
    }
}