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

        [AuthorizeRoles(Roles.All)]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string? name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .Include(i => i.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Name == name);

            return promocode is null ?
                ResponseUtil.NotFound(nameof(Promocode)) :
                ResponseUtil.Ok(promocode);
        }

        [AuthorizeRoles(Roles.All)]
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

        [AuthorizeRoles(Roles.All)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return await EndpointUtil.GetAll<PromocodeType>(_contextFactory);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(PromocodeDto promocode)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.PromocodeTypes.AnyAsync(a => a.Id == promocode.TypeId))
                return ResponseUtil.NotFound(nameof(PromocodeType));
            if (await context.Promocodes.AnyAsync(a => a.Name == promocode.Name))
                return ResponseUtil.Conflict("The promocode name is already in use");

            return await EndpointUtil.Create(promocode.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeDto promocodeDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .FirstOrDefaultAsync(f => f.Id == promocodeDto.Id);

            if (promocode is null)
                return ResponseUtil.NotFound(nameof(Promocode));
            if (!await context.PromocodeTypes.AnyAsync(a => a.Id == promocodeDto.TypeId))
                return ResponseUtil.NotFound(nameof(PromocodeType));
            if (promocode.Name != promocodeDto.Name && await context.Promocodes.AnyAsync(a => a.Name == promocodeDto.Name))
                return ResponseUtil.Conflict("The promocode name is already in use");

            return await EndpointUtil.Update(promocode, promocodeDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<Promocode>(id, context);
        }
    }
}
