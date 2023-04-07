using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/promocode")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public PromocodeController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<Promocode> promocodes = await context.Promocodes
                .Include(i => i.Type)
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(promocodes);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .Include(i => i.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return promocode is null ? 
                ResponseUtil.NotFound(nameof(Promocode)) : 
                ResponseUtil.Ok(promocode);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("type/{id}")]
        public async Task<IActionResult> GetType(Guid id)
        {
            return await EndpointUtil.GetById<PromocodeType>(id, _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return await EndpointUtil.GetAll<PromocodeType>(_contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(PromocodeDto promocode)
        {
            return await EndpointUtil.Create(promocode.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeDto promocode)
        {
            return await EndpointUtil.Update(promocode.Convert(false), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<Promocode>(id, _contextFactory);
        }
    }
}
