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

            return Ok(new
            {
                Data = await context.UserRestriction
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync(),
                Success = true
            });
        }

        [Authorize]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(new
            {
                Data = await context.UserRestriction
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RestrictionName == name && x.UserId == UserId),
                Success = true
            });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(UserRestriction restriction)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            restriction.Id = new Guid();
            restriction.CreatedDate = DateTime.UtcNow;

            await context.UserRestriction.AddAsync(restriction);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Restriction succesfully added", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(UserRestriction newRestriction)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRestriction? oldRestriction = await context.UserRestriction.FirstOrDefaultAsync(
                x => x.Id == newRestriction.Id && x.UserId == newRestriction.UserId);

            if (oldRestriction is null)
                return NotFound(new { Error = "Data was not found", Success = false });

            context.Entry(oldRestriction).CurrentValues.SetValues(newRestriction);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Restriction succesfully updated", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestriction
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (restriction is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.UserRestriction.Remove(restriction);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Restriction succesfully removed", Success = true });
        }
    }
}
