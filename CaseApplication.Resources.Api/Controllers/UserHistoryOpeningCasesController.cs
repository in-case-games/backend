using CaseApplication.Domain.Entities;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class UserHistoryOpeningCasesController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserHistoryOpeningCasesController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryOpeningCases? history = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return history is null ? NotFound() : Ok(history);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> histories = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .AsNoTracking()
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            return Ok(histories);
        }

        [AllowAnonymous]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllByUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> histories = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(histories);
        }

        [Authorize]
        [HttpGet("allHistory")]
        public async Task<IActionResult> GetAllHistory()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> histories = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .ToListAsync();

            return Ok(histories);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryOpeningCases? history = await context.UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);

            if (history is null) return NotFound();

            context.UserHistoryOpeningCases.Remove(history);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> histories = await context.UserHistoryOpeningCases
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            if (histories.Count == 0) return NotFound();

            context.UserHistoryOpeningCases.RemoveRange(histories);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
