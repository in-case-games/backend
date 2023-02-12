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
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfo
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.UserId == UserId);

            return info is null ? NotFound() : Ok(info);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateInfoByAdmin(UserAdditionalInfo newInfo)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? oldInfo = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == newInfo.UserId);

            if (oldInfo is null) return NotFound();

            context.Entry(oldInfo).CurrentValues.SetValues(newInfo);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
