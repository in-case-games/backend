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

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("{ownerId}&{userId}")]
        public async Task<IActionResult> GetByIds(Guid ownerId, Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.OwnerId == ownerId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
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

            return ResponseUtil.Ok(restrictions);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("owner/{userId}")]
        public async Task<IActionResult> GetByAdminAndUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            List<UserRestriction> restrictions = await context.UserRestrictions
                .Include(i => i.Type)
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.OwnerId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(restrictions);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetRestrictionType()
        {
            return await EndpointUtil.GetAll<RestrictionType>(_context);
        }

        [AllowAnonymous]
        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetRestrictionTypeById(Guid id)
        {
            return await EndpointUtil.GetById<RestrictionType>(id, _context);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            User? user;
            User? owner;

            try
            {
                user = await context.Users
                    .Include(i => i.AdditionalInfo)
                    .Include(i => i.AdditionalInfo!.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == restrictionDto.UserId);

                owner = await context.Users
                    .Include(i => i.AdditionalInfo)
                    .Include(i => i.AdditionalInfo!.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == UserId);
            }
            catch(ArgumentNullException)
            {
                return ResponseUtil.NotFound(nameof(UserRestriction));
            }

            string userRole = user!.AdditionalInfo!.Role!.Name!;
            string ownerRole = owner!.AdditionalInfo!.Role!.Name!;

            bool IsAccess = userRole == "user" || 
                ((ownerRole == "owner" || ownerRole == "bot") && userRole != "owner");

            if(IsAccess is false)
                return Forbid("Access denied");
            
            try
            {
                await context.UserRestrictions.AddAsync(restrictionDto.Convert());
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return ResponseUtil.Error(ex);
            }

            return ResponseUtil.Ok(restrictionDto);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserRestrictionDto restrictionDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == restrictionDto.Id);

            if(restriction == null) 
                return ResponseUtil.NotFound(nameof(UserRestriction));

            User? user;
            User? owner;

            try
            {
                user = await context.Users
                    .Include(i => i.AdditionalInfo)
                    .Include(i => i.AdditionalInfo!.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == restrictionDto.UserId);

                owner = await context.Users
                    .Include(i => i.AdditionalInfo)
                    .Include(i => i.AdditionalInfo!.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == UserId);
            }
            catch (ArgumentNullException)
            {
                return ResponseUtil.NotFound(nameof(UserRestriction));
            }

            string userRole = user!.AdditionalInfo!.Role!.Name!;
            string ownerRole = owner!.AdditionalInfo!.Role!.Name!;

            bool IsAccess = userRole == "user" || 
                ((ownerRole == "owner" || ownerRole == "bot") && userRole != "owner");

            if (IsAccess is false)
                return Forbid("Access denied");

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

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserRestriction? restriction = await context.UserRestrictions
                .FirstOrDefaultAsync(f => f.Id == id);

            if (restriction == null)
                return ResponseUtil.NotFound(nameof(UserRestriction));
            if (restriction.UserId == UserId)
                return Forbid("Access denied");

            context.UserRestrictions.Remove(restriction);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(restriction);
        }
    }
}
