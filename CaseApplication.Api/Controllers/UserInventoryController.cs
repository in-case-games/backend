using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
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

            if (inventory is null)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAll(Guid? userId = null)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventory
                .Include(x => x.GameItem)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok();
        }

        // TODO Sell and Withdrawn
    }
}
