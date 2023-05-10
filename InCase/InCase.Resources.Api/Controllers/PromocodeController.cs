using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
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

            Promocode promocode = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundCodeException("Промокод не найден");

            return ResponseUtil.Ok(promocode);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode promocode = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ??
                throw new NotFoundCodeException("Промокод не найден");

            return ResponseUtil.Ok(promocode);
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
                throw new BadRequestCodeException("Скидка промокода должна быть больше 0 и меньше 1");
            if (promocode.NumberActivations <= 0)
                throw new BadRequestCodeException("Количество активаций должно быть больше 0");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.PromocodeTypes.AnyAsync(pt => pt.Id == promocode.TypeId))
                throw new NotFoundCodeException("Тип промокода не найден");
            if (await context.Promocodes.AnyAsync(p => p.Name == promocode.Name))
                throw new ConflictCodeException("Имя промокода уже используется");

            return await EndpointUtil.Create(promocode.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeDto promocodeDto)
        {
            if (promocodeDto.Discount >= 1M || promocodeDto.Discount <= 0)
                throw new BadRequestCodeException("Скидка промокода должна быть больше 0 и меньше 1");
            if (promocodeDto.NumberActivations <= 0)
                throw new BadRequestCodeException("Количество активаций должно быть больше 0");

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .FirstOrDefaultAsync(p => p.Id == promocodeDto.Id);

            bool IsExist = await context.Promocodes.AnyAsync(p => p.Name == promocodeDto.Name);

            if (promocode is null)
                throw new NotFoundCodeException("Промокод не найден");
            if (!await context.PromocodeTypes.AnyAsync(pt => pt.Id == promocodeDto.TypeId))
                throw new NotFoundCodeException("Тип промокода не найден");
            if (promocode.Name != promocodeDto.Name && IsExist)
                throw new ConflictCodeException("Имя промокода уже используется");

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
