using CaseApplication.DomainLayer.Dtos;
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context
                .User
                .FirstOrDefaultAsync(x =>
                x.UserEmail == userDto.UserEmail ||
                x.Id == userDto.Id ||
                x.UserLogin == userDto.UserLogin);

            if (user == null) return NotFound();

            UserAdditionalInfo? userAdditionalInfo = await context.UserAdditionalInfo
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == UserId);
            
            if (userAdditionalInfo != null) return BadRequest();

            UserRole? searchRole = await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.RoleName == "user");

            if (searchRole is null) throw new Exception("Add standard roles to the database");

            UserAdditionalInfo info = new UserAdditionalInfo();

            info.Id = Guid.NewGuid();
            info.UserRoleId = searchRole!.Id;
            info.UserId = user.Id!;

            await context.UserAdditionalInfo.AddAsync(info);
            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateInfoByAdmin(UserAdditionalInfo info)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? searchInfo = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == info.UserId);

            if (searchInfo is null)
                return NotFound("There is no such information in the database, " +
                    "review what data comes from the api");

            context.Entry(searchInfo).CurrentValues.SetValues(info);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
