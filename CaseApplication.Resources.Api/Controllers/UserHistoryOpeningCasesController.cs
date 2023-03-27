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

            return history is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = history, Success = true });
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

            return Ok(new { Data = histories, Success = true });
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

            return Ok(new { Data = histories, Success = true });
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

            return Ok(new { Data = histories, Success = true });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryOpeningCases? history = await context.UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);

            if (history is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.UserHistoryOpeningCases.Remove(history);
            await context.SaveChangesAsync();

            return Ok(new { Message = "HistoryOpeningCase succesfully deleted", Success = true });
        }

        [Authorize]
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> histories = await context.UserHistoryOpeningCases
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            if (histories.Count == 0) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.UserHistoryOpeningCases.RemoveRange(histories);
            await context.SaveChangesAsync();

            return Ok(new { Message = "HistoryOpeningCases succesfully deleted", Success = true });
        }
    }
}
