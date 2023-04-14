using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
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
                .Include(i => i.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return restriction is null ? 
                ResponseUtil.NotFound(nameof(UserRestriction)) : 
                ResponseUtil.Ok(restriction);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return restrictions.Count == 0 ? 
                ResponseUtil.NotFound(nameof(UserRestriction)) : 
                ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(a => a.Id == id))
                return ResponseUtil.NotFound(nameof(User));

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == id)
                .ToListAsync();

            return restrictions.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserRestriction)) :
                ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}&{userId}")]
        public async Task<IActionResult> GetByIds(Guid ownerId, Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(a => a.Id == ownerId))
                return ResponseUtil.NotFound(nameof(User));
            if (!await context.Users.AnyAsync(a => a.Id == userId))
                return ResponseUtil.NotFound(nameof(User));

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.OwnerId == ownerId)
                .ToListAsync();

            return restrictions.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserRestriction)) :
                ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner")]
        public async Task<IActionResult> GetByAdmin()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.OwnerId == UserId)
                .ToListAsync();

            return restrictions.Count == 0 ?
                 ResponseUtil.NotFound(nameof(UserRestriction)) :
                 ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner/{userId}")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(a => a.Id == userId))
                return ResponseUtil.NotFound(nameof(User));

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.OwnerId == UserId)
                .ToListAsync();

            return restrictions.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserRestriction)) :
                ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType()
        {
            return await EndpointUtil.GetAll<RestrictionType>(_context);
        }

        //TODO CUT
        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            restrictionDto.OwnerId = UserId;

            RestrictionType? restrictionType = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.TypeId);

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .Include(i => i.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == restrictionDto.UserId);

            if (restrictionType is null)
                return ResponseUtil.NotFound(nameof(RestrictionType));
            if (user is null)
                return ResponseUtil.NotFound("User");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                return Forbid("Access denied");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == restrictionDto.UserId)
                .ToListAsync();

            int numberWarns = (restrictionType.Name == "warn") ? 1 : 0;

            foreach(var restriction in restrictions)
            {
                if (restriction.Type!.Name == "warn")
                    ++numberWarns;
            }

            if(numberWarns >= 3)
            {
                RestrictionType? ban = await context.RestrictionTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Name == "ban");

                restrictionDto.TypeId = ban!.Id;
                restrictionDto.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);
            }

            return await EndpointUtil.Create(restrictionDto.Convert(), context);
        }

        //TODO CUT
        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            restrictionDto.OwnerId = UserId;

            RestrictionType? restrictionType = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.TypeId);

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .Include(i => i.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == restrictionDto.UserId);

            if (restrictionType is null)
                return ResponseUtil.NotFound(nameof(RestrictionType));
            if (!await context.UserRestrictions.AnyAsync(a => a.Id == restrictionDto.Id))
                return ResponseUtil.NotFound("UserRestriction");
            if (user is null)
                return ResponseUtil.NotFound("User");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                return Forbid("Access denied");

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == restrictionDto.UserId)
                .ToListAsync();

            int numberWarns = (restrictionType.Name == "warn") ? 1 : 0;

            foreach (var restriction in restrictions)
            {
                if (restriction.Type!.Name == "warn" && restriction.Id != restrictionDto.Id)
                    ++numberWarns;
            }

            if (numberWarns >= 3)
            {
                RestrictionType? ban = await context.RestrictionTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Name == "ban");

                restrictionDto.TypeId = ban!.Id;
                restrictionDto.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);
            }

            return await EndpointUtil.Update(restrictionDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (restriction == null)
                return ResponseUtil.NotFound(nameof(UserRestriction));
            if (restriction.UserId == UserId)
                return Forbid("Access denied");

            return await EndpointUtil.Delete(restriction, context);
        }
    }
}
