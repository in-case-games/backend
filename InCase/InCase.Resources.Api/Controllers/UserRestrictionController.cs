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
    [Route("api/user-restriction")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserRestrictionController(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id);

            return restriction is null ? 
                ResponseUtil.NotFound("Эффект не найден") : 
                ResponseUtil.Ok(restriction);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (await context.Users.AnyAsync(u => u.Id == id))
                return ResponseUtil.NotFound("Пользователь не найден");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}&{userId}")]
        public async Task<IActionResult> GetByIds(Guid ownerId, Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (await context.Users.AnyAsync(u => u.Id == userId) is false)
                return ResponseUtil.NotFound("Пользователь не найден");
            if (await context.Users.AnyAsync(u => u.Id == ownerId) is false)
                return ResponseUtil.NotFound("Обвинитель не найден");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == ownerId && ur.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner")]
        public async Task<IActionResult> GetByAdmin()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner/{userId}")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (await context.Users.AnyAsync(u => u.Id == userId) is false)
                return ResponseUtil.NotFound("Пользователь не найден");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == UserId && ur.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType()
        {
            return await EndpointUtil.GetAll<RestrictionType>(_context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            restrictionDto.OwnerId = UserId;

            RestrictionType? type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == restrictionDto.TypeId);

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == restrictionDto.UserId);

            if (type is null)
                return ResponseUtil.NotFound("Тип эффекта не найден");
            if (user is null)
                return ResponseUtil.NotFound("Пользователь не найден");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                return ResponseUtil.Forbidden("Эффект можно наложить только на пользователя");

            restrictionDto = await CheckUserRestriction(restrictionDto, type, context);

            return await EndpointUtil.Create(restrictionDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            restrictionDto.OwnerId = UserId;

            RestrictionType? type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == restrictionDto.TypeId);

            User? user = await context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == restrictionDto.UserId);

            if (type is null)
                return ResponseUtil.NotFound("Тип эффекта не найден");
            if (await context.UserRestrictions.AnyAsync(a => a.Id == restrictionDto.Id) is false)
                return ResponseUtil.NotFound("Эффект не найден");
            if (user is null)
                return ResponseUtil.NotFound("Пользователь не найден");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                return ResponseUtil.Forbidden("Эффект можно наложить только на пользователя");

            restrictionDto = await CheckUserRestriction(restrictionDto, type, context);

            return await EndpointUtil.Update(restrictionDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id);

            return restriction is null ?
                ResponseUtil.NotFound("Эффект не найден") : 
                await EndpointUtil.Delete(restriction, context);
        }

        private static async Task<UserRestrictionDto> CheckUserRestriction(UserRestrictionDto restrictionDto,
            RestrictionType type,
            ApplicationDbContext context)
        {
            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == restrictionDto.UserId)
                .ToListAsync();

            int numberWarns = (type.Name == "warn") ? 1 : 0;

            foreach (var restriction in restrictions)
            {
                if (restriction.Type!.Name == "warn" && restriction.Id != restrictionDto.Id)
                    ++numberWarns;
            }

            if (numberWarns >= 3)
            {
                RestrictionType? ban = await context.RestrictionTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rt => rt.Name == "ban");

                restrictionDto.TypeId = ban!.Id;
                restrictionDto.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);
            }

            return restrictionDto;
        }
    }
}
