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

            GameItem? gameItem = await context.GameItem
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return gameItem is null ? NotFound(): Ok();
        }

        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? gameItem = await context.GameItem
                .AsNoTracking().FirstOrDefaultAsync(x => x.GameItemName == name);

            return gameItem is null ? NotFound() : Ok();
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.GameItem
                .AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(GameItem item)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            item.Id = Guid.NewGuid();

            await context.GameItem.AddAsync(item);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(GameItem item)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? searchItem = await context.GameItem
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (searchItem is null)
                return NotFound("There is no such item in the database, " +
                    "review what data comes from the api");

            context.Entry(searchItem).CurrentValues.SetValues(item);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? searchItem = await context.GameItem
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (searchItem is null)
                return NotFound("There is no such item in the database, " +
                    "review what data comes from the api");

            context.GameItem.Remove(searchItem);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
