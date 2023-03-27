using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public RoleController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRole? role = await context.UserRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleName == name);

            return role is null ?
                NotFound(new { Error = "Data was not found", Success = false }) :
                Ok(new { Data = role, Success = true });
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return Ok(new
            {
                Data = await context.UserRole
                .AsNoTracking()
                .ToListAsync(),
                Success = true
            });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> Create(UserRole role)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            role.Id = new Guid();

            await context.UserRole.AddAsync(role);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Role succesfully created", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> Update(UserRole role)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRole? oldRole = await context.UserRole
                .FirstOrDefaultAsync(x => x.Id == role.Id);

            if (oldRole is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.Entry(oldRole).CurrentValues.SetValues(role);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Role succesfully updated", Success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRole? role = await context.UserRole
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (role is null) 
                return NotFound(new { Error = "Data was not found", Success = false });

            context.UserRole.Remove(role);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Role succesfully deleted", Success = true });
        }
    }
}
