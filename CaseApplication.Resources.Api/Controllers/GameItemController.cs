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
    public class GameItemController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GameItemController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return item is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = item, Success = true });
        }

        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GameItemName == name);

            return item is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = item, Success = true });
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(new
            {
                Data = await context.GameItem
                .AsNoTracking()
                .ToListAsync(),
                Success = true
            });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(GameItem item)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            item.Id = Guid.NewGuid();

            await context.GameItem.AddAsync(item);
            await context.SaveChangesAsync();

            return Ok(new { Message = "GameItem succesfully created", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(GameItem newItem)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? oldItem = await context.GameItem
                .FirstOrDefaultAsync(x => x.Id == newItem.Id);

            if (oldItem is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.Entry(oldItem).CurrentValues.SetValues(newItem);
            await context.SaveChangesAsync();

            return Ok(new { Message = "GameItem succesfully updated", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItem
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item is null)
                return NotFound(new { Error = "Data was not found", Success = false });

            context.GameItem.Remove(item);
            await context.SaveChangesAsync();

            return Ok(new { Message = "GameItem succesfully deleted", Success = true });
        }
    }
}
