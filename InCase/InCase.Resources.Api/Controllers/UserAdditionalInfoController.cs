using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/user-additional-info")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserAdditionalInfoController(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .Include(i => i.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == UserId);

            return info is null ?
                ResponseUtil.NotFound(nameof(UserAdditionalInfo)) :
                ResponseUtil.Ok(info);
        }

        [AllowAnonymous]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            return await EndpointUtil.GetAll<UserRole>(_context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("guest-mode")]
        public async Task<IActionResult> OnOffGuestMode()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .Include(i => i.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == UserId);

            if (info is null)
                return ResponseUtil.NotFound(nameof(UserAdditionalInfo));

            info.IsGuestMode = !info.IsGuestMode;

            context.UserAdditionalInfos.Attach(info);
            context.Entry(info).Property(p => p.IsGuestMode).IsModified = true;

            await context.SaveChangesAsync();

            return info.IsGuestMode ? 
                ResponseUtil.Ok("On guest mode") : 
                ResponseUtil.Ok("Off guest mode");
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoDto infoDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.UserRoles.AnyAsync(a => a.Id == infoDto.RoleId))
                return ResponseUtil.NotFound(nameof(UserRole));
            if (!await context.Users.AnyAsync(a => a.Id == infoDto.UserId))
                return ResponseUtil.NotFound("User");

            return await EndpointUtil.Update(infoDto.Convert(false), context);
        }
    }
}
