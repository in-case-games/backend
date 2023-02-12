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
    public class UserRestrictionController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserRestrictionController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.UserRestriction
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync());
        }
        [Authorize]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(await context.UserRestriction
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RestrictionName == name && x.UserId == UserId));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(UserRestriction userRestriction)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            userRestriction.Id = new Guid();
            userRestriction.CreatedDate = DateTime.UtcNow;

            await context.UserRestriction.AddAsync(userRestriction);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(UserRestriction userRestriction)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRestriction? oldRestriction = await context.UserRestriction
                .FirstOrDefaultAsync(x => x.Id == userRestriction.Id);
            User? searchUser = await context.User
                .FirstOrDefaultAsync(x => x.Id == userRestriction.UserId);

            if (oldRestriction is null || searchUser is null)
                return NotFound("There is no such user restriction in the database, " +
                "review what data comes from the api");

            context.Entry(oldRestriction).CurrentValues.SetValues(userRestriction);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRestriction? searchRestriction = await context
                .UserRestriction
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchRestriction is null)
                return NotFound("There is no such user restriction in the database, " +
                "review what data comes from the api");

            context.UserRestriction.Remove(searchRestriction);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
