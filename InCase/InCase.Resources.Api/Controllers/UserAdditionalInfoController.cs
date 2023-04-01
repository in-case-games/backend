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
    [Route("api/user_additional_info")]
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
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == UserId);

            return info is null ?
                NotFound(new { Success = false, Data = "Data was not found" }) :
                Ok(new { Success = true, Data = info });
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpPut]
        public async Task<IActionResult> Update(UserAdditionalInfoDto infoDto)
        {
            await using ApplicationDbContext context = await _context.CreateDbContextAsync();

            UserAdditionalInfo? infoOld = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(x => x.Id == infoDto.Id);

            if (infoOld == null)
                return NotFound(new { Success = false, Data = "User not found the update is not available" });

            try
            {
                context.Entry(infoOld).CurrentValues.SetValues(infoDto.Convert());
                await context.SaveChangesAsync();
            }
            catch(Exception ex) {
                return Conflict(new
                {
                    Success = false,
                    Data = ex.InnerException!.Message.ToString()
                });
            }

            return Ok(new
            {
                Success = true,
                Data = infoDto.Convert()
            });
        }
    }
}
