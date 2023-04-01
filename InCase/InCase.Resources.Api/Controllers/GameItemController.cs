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

            List<GameItem> gameItems = await context.GameItems
                .AsNoTracking()
                .Include(x => x.Type)
                .ToListAsync();

            return ResponseUtil.Ok(gameItems);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? gameItem = await context.GameItems
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Rarity)
                .Include(x => x.Quality)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameItem is null)
                return ResponseUtil.NotFound(nameof(GameItem));

            return ResponseUtil.Ok(gameItem);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("qualities")]
        public async Task<IActionResult> GetQualities()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameItemQuality> qualities = await context.GameItemQualities
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(qualities);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameItemType> types = await context.GameItemTypes
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(types);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("rarities")]
        public async Task<IActionResult> GetRarities()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameItemRarity> rarities = await context.GameItemRarities
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(rarities);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(GameItemDto gameItem)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.GameItems.AddAsync(gameItem.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(gameItem);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpPost("qualities")]
        public async Task<IActionResult> CreateQuality(GameItemQuality quality)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.GameItemQualities.AddAsync(quality);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(quality);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpPost("rarities")]
        public async Task<IActionResult> CreateRarity(GameItemRarity rarity)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.GameItemRarities.AddAsync(rarity);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(rarity);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpPost("types")]
        public async Task<IActionResult> CreateRarity(GameItemType type)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.GameItemTypes.AddAsync(type);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(type);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(GameItemDto item)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? oldItem = await context.GameItems
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (oldItem is null)
                return ResponseUtil.NotFound(nameof(GameItem));

            try
            {
                context.Entry(oldItem).CurrentValues.SetValues(item.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(item);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? gameItem = await context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameItem is null)
                return ResponseUtil.NotFound(nameof(GameItem));

            context.GameItems.Remove(gameItem);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(GameItem));
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpDelete("qualities/{id}")]
        public async Task<IActionResult> DeleteItemQuality(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItemQuality? gameItemQuality = await context.GameItemQualities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameItemQuality is null)
                return ResponseUtil.NotFound(nameof(GameItemQuality));

            context.GameItemQualities.Remove(gameItemQuality);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(GameItemQuality));
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpDelete("rarities/{id}")]
        public async Task<IActionResult> DeleteItemRarity(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItemRarity? gameItemRarity = await context.GameItemRarities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameItemRarity is null)
                return ResponseUtil.NotFound(nameof(GameItemRarity));

            context.GameItemRarities.Remove(gameItemRarity);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(GameItemRarity));
        }
        [AuthorizeRoles(Roles.Admin)]
        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteItemType(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItemType? gameItemType = await context.GameItemTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gameItemType is null)
                return ResponseUtil.NotFound(nameof(GameItemType));

            context.GameItemTypes.Remove(gameItemType);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(GameItemType));
        }
    }
}
