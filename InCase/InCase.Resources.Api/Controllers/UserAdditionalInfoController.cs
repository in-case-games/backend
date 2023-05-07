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
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId);

            return info is null ?
                ResponseUtil.NotFound("Дополнительная информация не найдена") :
                ResponseUtil.Ok(info);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.Id == id);

            return info is null ?
                ResponseUtil.NotFound("Дополнительная информация не найдена") :
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
                .Include(uai => uai.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId);

            if (info is null)
                return ResponseUtil.NotFound("Дополнительная информация не найдена");

            info.IsGuestMode = !info.IsGuestMode;

            context.UserAdditionalInfos.Attach(info);
            context.Entry(info).Property(p => p.IsGuestMode).IsModified = true;

            await context.SaveChangesAsync();

            return info.IsGuestMode ? 
                ResponseUtil.Ok("Гостевой мод включен") : 
                ResponseUtil.Ok("Гостевой мод выключен");
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoDto infoDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.UserRoles.AnyAsync(ur => ur.Id == infoDto.RoleId))
                return ResponseUtil.NotFound("Роль не найдена");
            if (!await context.Users.AnyAsync(u => u.Id == infoDto.UserId))
                return ResponseUtil.NotFound("Пользователь не найден");

            return await EndpointUtil.Update(infoDto.Convert(false), context);
        }
    }
}
