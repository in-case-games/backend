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
                .Include(i => i.Type)
                .Include(i => i.Quality)
                .Include(i => i.Rarity)
                .AsNoTracking()
                .ToListAsync();

            return items.Count == 0 ? 
                ResponseUtil.NotFound(nameof(GameItem)) : 
                ResponseUtil.Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItems
                .Include(i => i.Type)
                .Include(i => i.Rarity)
                .Include(i => i.Quality)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return item is null ? 
                ResponseUtil.NotFound(nameof(GameItem)) : 
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

            if (!await context.Games.AnyAsync(a => a.Id == itemDto.GameId))
                return ResponseUtil.NotFound(nameof(Game));
            if (!await context.GameItemTypes.AnyAsync(a => a.Id == itemDto.TypeId))
                return ResponseUtil.NotFound(nameof(GameItemType));
            if (!await context.GameItemRarities.AnyAsync(a => a.Id == itemDto.RarityId))
                return ResponseUtil.NotFound(nameof(GameItemRarity));
            if (!await context.GameItemQualities.AnyAsync(a => a.Id == itemDto.QualityId))
                return ResponseUtil.NotFound(nameof(GameItemQuality));

            return await EndpointUtil.Create(itemDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto itemDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(a => a.Id == itemDto.GameId))
                return ResponseUtil.NotFound(nameof(Game));
            if (!await context.GameItemTypes.AnyAsync(a => a.Id == itemDto.TypeId))
                return ResponseUtil.NotFound(nameof(GameItemType));
            if (!await context.GameItemRarities.AnyAsync(a => a.Id == itemDto.RarityId))
                return ResponseUtil.NotFound(nameof(GameItemRarity));
            if (!await context.GameItemQualities.AnyAsync(a => a.Id == itemDto.QualityId))
                return ResponseUtil.NotFound(nameof(GameItemQuality));

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
