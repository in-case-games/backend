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

            List<LootBox> boxes = await context.LootBoxes
                .AsNoTracking()
                .ToListAsync();

            List<LootBoxDto> boxesDtos = new();
            boxes.ForEach(lb => boxesDtos.Add(lb.Convert(false)));

            return ResponseUtil.Ok(boxesDtos);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? box = await context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id);

            return box is null? 
                ResponseUtil.NotFound("Кейс не найден") : 
                ResponseUtil.Ok(box.Convert(false));
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetByAdmin(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? box = await context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id);

            return box is null ? 
                ResponseUtil.NotFound("Кейс не найден") : 
                ResponseUtil.Ok(box);
        }

        [AllowAnonymous]
        [HttpGet("{id}/inventory")]
        public async Task<IActionResult> GetInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (await context.LootBoxes.AnyAsync(lb => lb.Id == id) is false)
                return ResponseUtil.NotFound("Кейс не найден");

            List<LootBoxInventory> inventories = await context.LootBoxInventories
                .Include(lbi => lbi.Item)
                .Include(lbi => lbi.Item!.Type)
                .Include(lbi => lbi.Item!.Rarity)
                .Include(lbi => lbi.Item!.Quality)
                .AsNoTracking()
                .Where(lbi => lbi.BoxId == id)
                .ToListAsync();

            return ResponseUtil.Ok(inventories);
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

            List<LootBoxBannerDto> result = new();

            banners.ForEach(b => result.Add(b.Convert(false)));

            return ResponseUtil.Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}/banner")]
        public async Task<IActionResult> GetBannerById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner? banner = await context.LootBoxBanners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.BoxId == id);

            return banner is null ? 
                ResponseUtil.NotFound("Баннер не найден") : 
                ResponseUtil.Ok(banner.Convert(false));
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxDto boxDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Games.AnyAsync(g => g.Id == boxDto.GameId))
                return ResponseUtil.NotFound("Игра не найдена");

            return await EndpointUtil.Create(boxDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("inventory")]
        public async Task<IActionResult> CreateInventory(LootBoxInventoryDto inventoryDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.LootBoxes.AnyAsync(lb => lb.Id == inventoryDto.BoxId))
                return ResponseUtil.NotFound("Кейс не найден");
            if (!await context.GameItems.AnyAsync(gi => gi.Id == inventoryDto.ItemId))
                return ResponseUtil.NotFound("Предмет не найден");

            return await EndpointUtil.Create(inventoryDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPost("banner")]
        public async Task<IActionResult> CreateBanner(LootBoxBannerDto bannerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.LootBoxes.AnyAsync(lb => lb.Id == bannerDto.BoxId))
                return ResponseUtil.NotFound("Кейс не найден");
            if (await context.LootBoxBanners.AnyAsync(a => a.BoxId == bannerDto.BoxId))
                return ResponseUtil.Conflict("Кейс уже использует баннер");

            return await EndpointUtil.Create(bannerDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(LootBoxDto boxDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? oldBox = await context.LootBoxes
                .FirstOrDefaultAsync(lb => lb.Id == boxDto.Id);

            if (oldBox is null)
                return ResponseUtil.NotFound("Кейс не найден");
            if (await context.Games.AnyAsync(g => g.Id == boxDto.GameId) is false)
                return ResponseUtil.NotFound("Игра не найдена");

            LootBox? newBox = boxDto.Convert(false);
            newBox.VirtualBalance = oldBox.VirtualBalance;
            newBox.Balance = oldBox.Balance;

            return await EndpointUtil.Update(oldBox, newBox, context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpPut("banner")]
        public async Task<IActionResult> UpdateBanner(LootBoxBannerDto bannerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner? banner = await context.LootBoxBanners
                .FirstOrDefaultAsync(lbb => lbb.Id == bannerDto.Id);

            bool IsUsedBox = await context.LootBoxBanners.AnyAsync(lbb => lbb.BoxId == bannerDto.BoxId);

            if (banner is null)
                return ResponseUtil.NotFound("Баннер не найден");
            if (!await context.LootBoxes.AnyAsync(lb => lb.Id == bannerDto.BoxId))
                return ResponseUtil.NotFound("Кейс не найден");
            if (banner.BoxId != bannerDto.BoxId && IsUsedBox)
                return ResponseUtil.Conflict("Баннер уже использует кейс");

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
