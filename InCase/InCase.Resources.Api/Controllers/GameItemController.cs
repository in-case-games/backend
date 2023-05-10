using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/game-item")]
    [ApiController]
    public class GameItemController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public GameItemController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<GameItem> items = await context.GameItems
                .Include(i => i.Type)
                .Include(i => i.Quality)
                .Include(i => i.Rarity)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            GameItem item = await context.GameItems
                .Include(i => i.Type)
                .Include(i => i.Rarity)
                .Include(i => i.Quality)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Предмет не найден");

            return ResponseUtil.Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities(CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetAll<GameItemQuality>(_contextFactory, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes(CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetAll<GameItemType>(_contextFactory, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities(CancellationToken cancellationToken)
        {
            return await EndpointUtil.GetAll<GameItemRarity>(_contextFactory, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(GameItemDto itemDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            await CheckGameItem(itemDto, context);

            return await EndpointUtil.Create(itemDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto itemDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            await CheckGameItem(itemDto, context);

            return await EndpointUtil.Update(itemDto.Convert(false), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<GameItem>(id, context, cancellationToken);
        }

        private static async Task CheckGameItem(GameItemDto itemDto, ApplicationDbContext context)
        {
            if (!await context.Games.AnyAsync(a => a.Id == itemDto.GameId))
                throw new NotFoundCodeException("Игра не найден");
            if (!await context.GameItemTypes.AnyAsync(a => a.Id == itemDto.TypeId))
                throw new NotFoundCodeException("Тип предмета не найден");
            if (!await context.GameItemRarities.AnyAsync(a => a.Id == itemDto.RarityId))
                throw new NotFoundCodeException("Редкость предмета не найдена");
            if (!await context.GameItemQualities.AnyAsync(a => a.Id == itemDto.QualityId))
                throw new NotFoundCodeException("Качество предмета не найдено");
        }
    }
}
