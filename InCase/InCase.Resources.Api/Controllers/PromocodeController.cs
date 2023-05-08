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
                .Include(p => p.Type)
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
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name);

            return promocode is null ?
                ResponseUtil.NotFound("Промокод не найден") :
                ResponseUtil.Ok(promocode);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return promocode is null ? 
                ResponseUtil.NotFound("Промокод не найден") : 
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
            if (promocode.Discount >= 1M || promocode.Discount <= 0)
                return ResponseUtil.BadRequest("Скидка промокода должна быть больше 0 и меньше 1");
            if (promocode.NumberActivations <= 0)
                return ResponseUtil.BadRequest("Количество активаций должно быть больше 0");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.PromocodeTypes.AnyAsync(pt => pt.Id == promocode.TypeId))
                return ResponseUtil.NotFound("Тип промокода не найден");
            if (await context.Promocodes.AnyAsync(p => p.Name == promocode.Name))
                return ResponseUtil.Conflict("Имя промокода уже используется");

            return await EndpointUtil.Create(promocode.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeDto promocodeDto)
        {
            if (promocodeDto.Discount >= 1M || promocodeDto.Discount <= 0)
                return ResponseUtil.BadRequest("Скидка промокода должна быть больше 0 и меньше 1");
            if (promocodeDto.NumberActivations <= 0)
                return ResponseUtil.BadRequest("Количество активаций должно быть больше 0");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .FirstOrDefaultAsync(p => p.Id == promocodeDto.Id);

            bool IsExist = await context.Promocodes.AnyAsync(p => p.Name == promocodeDto.Name);

            if (promocode is null)
                return ResponseUtil.NotFound("Промокод не найден");
            if (!await context.PromocodeTypes.AnyAsync(pt => pt.Id == promocodeDto.TypeId))
                return ResponseUtil.NotFound("Тип промокода не найден");
            if (promocode.Name != promocodeDto.Name && IsExist)
                return ResponseUtil.Conflict("Имя промокода уже используется");

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
