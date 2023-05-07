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
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameItem> items = await context.GameItems
                .Include(gi => gi.Type)
                .Include(gi => gi.Quality)
                .Include(gi => gi.Rarity)
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItems
                .Include(gi => gi.Type)
                .Include(gi => gi.Rarity)
                .Include(gi => gi.Quality)
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id);

            return item is null ? 
                ResponseUtil.NotFound("Предмет не найден") : 
                ResponseUtil.Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities()
        {
            return await EndpointUtil.GetAll<GameItemQuality>(_contextFactory);
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return await EndpointUtil.GetAll<GameItemType>(_contextFactory);
        }

        [AllowAnonymous]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            return await EndpointUtil.GetAll<GameItemRarity>(_contextFactory);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(GameItemDto itemDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(g => g.Id == itemDto.GameId))
                return ResponseUtil.NotFound("Игра не найдена");
            if (!await context.GameItemTypes.AnyAsync(git => git.Id == itemDto.TypeId))
                return ResponseUtil.NotFound("Тип предмета не найден");
            if (!await context.GameItemRarities.AnyAsync(gir => gir.Id == itemDto.RarityId))
                return ResponseUtil.NotFound("Редкость предмета не найдена");
            if (!await context.GameItemQualities.AnyAsync(giq => giq.Id == itemDto.QualityId))
                return ResponseUtil.NotFound("Качество предмета не найдено");

            return await EndpointUtil.Create(itemDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto itemDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(g => g.Id == itemDto.GameId))
                return ResponseUtil.NotFound("Игра не найдена");
            if (!await context.GameItemTypes.AnyAsync(git => git.Id == itemDto.TypeId))
                return ResponseUtil.NotFound("Тип предмета не найден");
            if (!await context.GameItemRarities.AnyAsync(gir => gir.Id == itemDto.RarityId))
                return ResponseUtil.NotFound("Редкость предмета не найдена");
            if (!await context.GameItemQualities.AnyAsync(giq => giq.Id == itemDto.QualityId))
                return ResponseUtil.NotFound("Качество предмета не найдено");

            return await EndpointUtil.Update(itemDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<GameItem>(id, context);
        }
    }
}
