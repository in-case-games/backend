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
    public class UserInventoryController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserInventoryController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) 
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? inventory = await context.UserInventory
                .Include(x => x.GameItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return inventory is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = inventory, Success = true });
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventory
                .Include(x => x.GameItem)
                .AsNoTracking()
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            return Ok(new { Data = inventories, Success = true });
        }

        [Authorize]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllByUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventory
                .Include(x => x.GameItem)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(new { Data = inventories, Success = true });
        }
    }
}
