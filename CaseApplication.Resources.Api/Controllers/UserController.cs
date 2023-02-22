using CaseApplication.Domain.Entities.Email;
using CaseApplication.Domain.Entities.Resources;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [ApiController]
    [Route("resources/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailHelper _emailHelper;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(
            IDbContextFactory<ApplicationDbContext> contextFactory, 
            EmailHelper emailHelper)
        {
            _contextFactory = contextFactory;
            _emailHelper = emailHelper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null) return NotFound();
            
            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound();

            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";

            return Ok(user);
        }

        [Authorize]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserLogin == login);

            if (user == null) return NotFound();
            
            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<User> users = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .ToListAsync();

            for(int i = 0; i < users.Count; i++)
            {
                users[i].UserEmail = "access denied";
                users[i].PasswordHash = "access denied";
                users[i].PasswordSalt = "access denied";
            }

            return Ok(users);
        }

        [Authorize]
        [HttpPut("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool isExistLogin = await context.User.AnyAsync(x => x.UserLogin == login);

            User? user = await context.User
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (isExistLogin) return BadRequest();
            if (user == null) return NotFound();

            user.UserLogin = login;
            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailTemplate()
                {
                    BodyDescription = $"Имя вашего акканута измененно на: {login}"
                });

            return Ok(new { Success = true, Message = "Account properties was changed.", Changed = "Login" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user is null) return NotFound();

            context.User.Remove(user);
            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Account succesfully deleted."});
        }
    }
}
