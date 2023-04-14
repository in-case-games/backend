using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/loot-box")]
    [ApiController]
    public class LootBoxController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public LootBoxController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<LootBox> lootBoxes = await context.LootBoxes
                .AsNoTracking()
                .ToListAsync();

            foreach(var lootBox in lootBoxes)
            {
                lootBox.Balance = 0;
                lootBox.VirtualBalance = 0;
            }

            return lootBoxes.Count == 0 ? 
                ResponseUtil.NotFound(nameof(LootBox)) : 
                ResponseUtil.Ok(lootBoxes);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? lootBox = await context.LootBoxes
                .AsNoTracking()
                .Include(i => i.Inventories)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (lootBox is null)
                ResponseUtil.NotFound(nameof(LootBox));

            lootBox!.Balance = 0;
            lootBox!.VirtualBalance = 0;

            return ResponseUtil.Ok(lootBox);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? lootBox = await context.LootBoxes
                .AsNoTracking()
                .Include(i => i.Inventories)
                .FirstOrDefaultAsync(f => f.Id == id);

            return lootBox is null ? 
                ResponseUtil.NotFound(nameof(LootBox)) : 
                ResponseUtil.Ok(lootBox);
        }

        [AllowAnonymous]
        [HttpGet("{id}/inventory")]
        public async Task<IActionResult> GetInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.LootBoxes.AnyAsync(x => x.Id == id))
                return ResponseUtil.NotFound(nameof(LootBox));

            List<LootBoxInventory> inventories = await context.LootBoxInventories
                .Include(i => i.Item)
                .AsNoTracking()
                .Where(w => w.BoxId == id)
                .ToListAsync();

            return inventories.Count == 0? 
                ResponseUtil.NotFound(nameof(LootBoxInventory)) : 
                ResponseUtil.Ok(inventories);
        }

        [AllowAnonymous]
        [HttpGet("banners")]
        public async Task<IActionResult> GetBanners()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<LootBoxBanner> banners = await context.LootBoxBanners
                .Include(i => i.Box)
                .AsNoTracking()
                .ToListAsync();

            foreach(var banner in banners)
            {
                banner.Box!.Balance = 0;
                banner.Box!.VirtualBalance = 0;
            }

            return banners.Count == 0 ? 
                ResponseUtil.NotFound(nameof(LootBoxBanner)) : 
                ResponseUtil.Ok(banners);
        }

        [AllowAnonymous]
        [HttpGet("{id}/banner")]
        public async Task<IActionResult> GetBannerById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner? banner = await context.LootBoxBanners
                .Include(i => i.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BoxId == id);

            if (banner is null)
                ResponseUtil.NotFound(nameof(LootBoxBanner));

            banner!.Box!.Balance = 0;
            banner!.Box!.VirtualBalance = 0;

            return ResponseUtil.Ok(banner);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxDto boxDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(a => a.Id == boxDto.GameId))
                return ResponseUtil.NotFound(nameof(Game));

            return await EndpointUtil.Create(boxDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("inventory")]
        public async Task<IActionResult> CreateInventory(LootBoxInventoryDto inventoryDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.LootBoxes.AnyAsync(a => a.Id == inventoryDto.BoxId))
                return ResponseUtil.NotFound(nameof(LootBox));
            if (!await context.GameItems.AnyAsync(a => a.Id == inventoryDto.ItemId))
                return ResponseUtil.NotFound(nameof(GameItem));

            return await EndpointUtil.Create(inventoryDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("banner")]
        public async Task<IActionResult> CreateBanner(LootBoxBannerDto bannerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.LootBoxes.AnyAsync(a => a.Id == bannerDto.BoxId))
                return ResponseUtil.NotFound(nameof(LootBox));
            if (await context.LootBoxBanners.AnyAsync(a => a.BoxId == bannerDto.BoxId))
                return ResponseUtil.Conflict("The banner is already used by this loot box");

            return await EndpointUtil.Create(bannerDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(LootBoxDto boxDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(a => a.Id == boxDto.GameId))
                return ResponseUtil.NotFound(nameof(Game));

            return await EndpointUtil.Update(boxDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut("banner")]
        public async Task<IActionResult> UpdateBanner(LootBoxBannerDto bannerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner? banner = await context.LootBoxBanners
                .FirstOrDefaultAsync(f => f.Id == bannerDto.Id);

            if(banner is null)
                return ResponseUtil.NotFound(nameof(LootBoxBanner));
            if (!await context.LootBoxes.AnyAsync(a => a.Id == bannerDto.BoxId))
                return ResponseUtil.NotFound(nameof(LootBox));
            if (banner.BoxId != bannerDto.BoxId && await context.LootBoxBanners.AnyAsync(a => a.BoxId == bannerDto.BoxId))
                return ResponseUtil.Conflict("The banner is already used by this loot box");

            return await EndpointUtil.Update(banner, bannerDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<LootBox>(id, context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("banner/{id}")]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<LootBoxBanner>(id, context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("inventory/{id}")]
        public async Task<IActionResult> DeleteItemFromInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<LootBoxInventory>(id, context);
        }
    }
}
