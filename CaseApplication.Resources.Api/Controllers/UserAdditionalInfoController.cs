using AutoMapper;
using CaseApplication.Domain.Dtos;
using CaseApplication.Domain.Entities.Internal;
using CaseApplication.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Resources.Api.Controllers
{
    [Route("resources/api/[controller]")]
    [ApiController]
    public class UserAdditionalInfoController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<UserAdditionalInfoDto, UserAdditionalInfo>();
        });

        public UserAdditionalInfoController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfo
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.UserId == UserId);

            return info is null ? NotFound() : Ok(info);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("admin")]
        public async Task<IActionResult> UpdateInfoByAdmin(UserAdditionalInfoDto newInfoDto)
        {
            IMapper mapper = _mapperConfiguration.CreateMapper();
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? oldInfo = await context.UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.Id == newInfoDto.Id);

            if (oldInfo is null) return NotFound();

            UserAdditionalInfo newInfo = mapper.Map<UserAdditionalInfo>(newInfoDto);

            context.Entry(oldInfo).CurrentValues.SetValues(newInfo);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
