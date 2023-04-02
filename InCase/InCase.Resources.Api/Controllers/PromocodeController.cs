using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public PromocodeController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<Promocode> promocodes = await context.Promocodes
                .AsNoTracking()
                .Include(x => x.Type)
                .ToListAsync();

            return ResponseUtil.Ok(promocodes);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .AsNoTracking()
                .Include(x => x.Type)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (promocode is null)
                return ResponseUtil.NotFound(nameof(GameItem));

            return ResponseUtil.Ok(promocode);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return await EndpointUtil.GetAll<PromocodeType>(_contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetType(Guid id)
        {
            return await EndpointUtil.GetById<PromocodeType>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(PromocodeDto promocode)
        {
            return await EndpointUtil.Create(promocode.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost("types")]
        public async Task<IActionResult> CreateType(PromocodeType type)
        {
            return await EndpointUtil.Create(type, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeDto promocode)
        {
            return await EndpointUtil.Update(promocode.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPut("types")]
        public async Task<IActionResult> UpdateType(PromocodeType type)
        {
            return await EndpointUtil.Update(type, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<Promocode>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteType(Guid id)
        {
            return await EndpointUtil.Delete<PromocodeType>(id, _contextFactory);
        }
    }
}
