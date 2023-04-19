using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
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

            User? user = await context.Users
               .Include(i => i.Restrictions!)
                   .ThenInclude(ti => ti.Type)
               .AsNoTracking()
               .FirstOrDefaultAsync(f => f.Id == id);

            if (user is null)
                return ResponseUtil.NotFound("User");

            return user.Restrictions is null || user.Restrictions.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserRestriction)) : 
                ResponseUtil.Ok(user.Restrictions);
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}&{userId}")]
        public async Task<IActionResult> GetByIds(Guid ownerId, Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            User? user = await context.Users
               .Include(i => i.Restrictions!)
                   .ThenInclude(ti => ti.Type)
               .AsNoTracking()
               .FirstOrDefaultAsync(f => f.Id == userId);

            if (user is null)
                return ResponseUtil.NotFound("User");
            if (user.Restrictions is null)
                return ResponseUtil.NotFound(nameof(UserRestriction));

            List<UserRestriction> restrictions = user.Restrictions
                .Where(w => w.OwnerId == ownerId)
                .ToList();

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

            User? user = await context.Users
               .Include(i => i.OwnerRestrictions!)
                   .ThenInclude(ti => ti.Type)
               .AsNoTracking()
               .FirstOrDefaultAsync(f => f.Id == UserId);

            if (user!.OwnerRestrictions is null)
                return ResponseUtil.NotFound(nameof(UserRestriction));

            List<UserRestriction> restrictions = user.OwnerRestrictions
                .Where(w => w.UserId == userId)
                .ToList();

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

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            restrictionDto.OwnerId = UserId;

            RestrictionType? type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.TypeId);

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .Include(i => i.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.UserId);

            if (type is null)
                return ResponseUtil.NotFound(nameof(RestrictionType));
            if (user is null)
                return ResponseUtil.NotFound("User");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                return Conflict ("Access denied");

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
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.TypeId);

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .Include(i => i.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.UserId);

            if (type is null)
                return ResponseUtil.NotFound(nameof(RestrictionType));
            if (!await context.UserRestrictions.AnyAsync(a => a.Id == restrictionDto.Id))
                return ResponseUtil.NotFound(nameof(UserRestriction));
            if (user is null)
                return ResponseUtil.NotFound("User");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                return Forbid("Access denied");

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
                .FirstOrDefaultAsync(f => f.Id == id);

            if (restriction == null)
                return ResponseUtil.NotFound(nameof(UserRestriction));
            if (restriction.UserId == UserId)
                return Forbid("Access denied");

            return await EndpointUtil.Delete(restriction, context);
        }

        private static async Task<UserRestrictionDto> CheckUserRestriction(UserRestrictionDto restrictionDto,
            RestrictionType type,
            ApplicationDbContext context)
        {
            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == restrictionDto.UserId)
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
                    .FirstOrDefaultAsync(f => f.Name == "ban");

                restrictionDto.TypeId = ban!.Id;
                restrictionDto.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);
            }

            return restrictionDto;
        }
    }
}
