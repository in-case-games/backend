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

            UserHistoryOpeningCases? userHistory = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return userHistory is null ? NotFound() : Ok(userHistory);
        }

        [AllowAnonymous]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllByUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> userHistories = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return userHistories is null ? NotFound() : Ok(userHistories);
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases> userHistories = await context.UserHistoryOpeningCases
                .Include(x => x.GameCase)
                .Include(x => x.GameItem)
                .ToListAsync();

            return userHistories is null ? NoContent() : Ok(userHistories);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryOpeningCases? searchUserHistory = await context
                .UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserHistory is null)
                return NotFound("There is no such user history, " +
                "review what data comes from the api");

            context.UserHistoryOpeningCases.Remove(searchUserHistory);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases>? userHistories = await context.UserHistoryOpeningCases
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            if (userHistories.Count == 0)
                return NotFound("There is no such user history, " +
                "review what data comes from the api");

            context.UserHistoryOpeningCases.RemoveRange(userHistories);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
