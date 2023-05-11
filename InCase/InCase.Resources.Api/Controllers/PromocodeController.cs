using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
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
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<Promocode> promocodes = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(promocodes);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string? name, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            Promocode promocode = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name, cancellationToken) ??
                throw new NotFoundCodeException("Промокод не найден");

            return ResponseUtil.Ok(promocode);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            Promocode promocode = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Промокод не найден");

            return ResponseUtil.Ok(promocode);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes(CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetAll<PromocodeType>(_contextFactory, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(PromocodeDto promocode, CancellationToken cancellationToken)
        {
            ValidationService.CheckBadRequestPromocode(promocode);

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            if (!await context.PromocodeTypes.AnyAsync(pt => pt.Id == promocode.TypeId, cancellationToken))
                throw new NotFoundCodeException("Тип промокода не найден");
            if (await context.Promocodes.AnyAsync(p => p.Name == promocode.Name, cancellationToken))
                throw new ConflictCodeException("Имя промокода уже используется");

            return await EndpointUtil.Create(promocode.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeDto promocodeDto, CancellationToken cancellationToken)
        {
            ValidationService.CheckBadRequestPromocode(promocodeDto);

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            Promocode? promocode = await context.Promocodes
                .FirstOrDefaultAsync(p => p.Id == promocodeDto.Id, cancellationToken);

            bool isExist = await context.Promocodes.AnyAsync(p => p.Name == promocodeDto.Name, cancellationToken);

            if (promocode is null)
                throw new NotFoundCodeException("Промокод не найден");
            if (!await context.PromocodeTypes.AnyAsync(pt => pt.Id == promocodeDto.TypeId, cancellationToken))
                throw new NotFoundCodeException("Тип промокода не найден");
            if (promocode.Name != promocodeDto.Name && isExist)
                throw new ConflictCodeException("Имя промокода уже используется");

            return await EndpointUtil.Update(promocode,
                                        promocodeDto.Convert(false),
                                        context,
                                        cancellationToken);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<Promocode>(id, context, cancellationToken);
        }
    }
}
