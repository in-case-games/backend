using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
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

            GameItem? item = await context.GameItems
                .Include(i => i.Type)
                .Include(i => i.Rarity)
                .Include(i => i.Quality)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            return item is null ? 
                ResponseUtil.NotFound("Предмет не найден") : 
                ResponseUtil.Ok(item);
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

            string notFoundItem = await NotFoundGameItem(itemDto, context);

            if (notFoundItem is not "")
                return ResponseUtil.NotFound(notFoundItem);

            return await EndpointUtil.Create(itemDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto itemDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            string notFoundItem = await NotFoundGameItem(itemDto, context);

            if (notFoundItem is not "")
                return ResponseUtil.NotFound(notFoundItem);

            return await EndpointUtil.Update(itemDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<GameItem>(id, context, cancellationToken);
        }
        private async Task<string> NotFoundGameItem(GameItemDto itemDto, ApplicationDbContext context)
        {
            if (!await context.Games.AnyAsync(a => a.Id == itemDto.GameId))
                return "Игра не найдена";
            if (!await context.GameItemTypes.AnyAsync(a => a.Id == itemDto.TypeId))
                return "Тип предмета не найден";
            if (!await context.GameItemRarities.AnyAsync(a => a.Id == itemDto.RarityId))
                return "Редкость предмета не найдена";
            if (!await context.GameItemQualities.AnyAsync(a => a.Id == itemDto.QualityId))
                return "Качество предмета не найдено";

            return "";
        }
    }
}
