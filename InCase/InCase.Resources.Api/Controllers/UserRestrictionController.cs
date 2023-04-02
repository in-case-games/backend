using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                .Include(x => x.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return ResponseUtil.Ok(restriction);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(x => x.Type)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(x => x.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}&{userId}")]
        public async Task<IActionResult> GetByOwnerIdAndUserId(Guid ownerId, Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(x => x.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.OwnerId == ownerId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(x => x.Type)
                .AsNoTracking()
                .Where(w => w.OwnerId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpGet("admin/{userId}")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(x => x.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.OwnerId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType()
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<RestrictionType> types = await context.RestrictionTypes
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(types);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetRestrictionTypeById(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            RestrictionType? type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return ResponseUtil.Ok(type);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            try
            {
                await context.UserRestrictions.AddAsync(restrictionDto.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(restrictionDto);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .FirstOrDefaultAsync(x => x.Id == restrictionDto.Id);

            if (restriction == null) 
                return ResponseUtil.NotFound(nameof(UserRestriction));

            try
            {
                context.Entry(restriction).CurrentValues.SetValues(restrictionDto.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(restrictionDto);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (restriction == null)
                return ResponseUtil.NotFound(nameof(UserRestriction));

            context.UserRestrictions.Remove(restriction);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(restriction);
        }
    }
}
