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
    [Route("api/lootbox")]
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
                .Include(x => x.Game)
                .ToListAsync();

            return ResponseUtil.Ok(lootBoxes);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? lootBox = await context.LootBoxes
                .AsNoTracking()
                .Include(x => x.Game)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));

            return ResponseUtil.Ok(lootBox);
        }
        [AllowAnonymous]
        [HttpGet("inventory/{id}")]
        public async Task<IActionResult> GetInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<LootBoxInventory> boxInventories = await context.LootBoxInventories
                .AsNoTracking()
                .Include(x => x.Item)
                .Where(x => x.BoxId == id)
                .ToListAsync();

            return ResponseUtil.Ok(boxInventories);
        }
        [AllowAnonymous]
        [HttpGet("banners/{id}")]
        public async Task<IActionResult> GetBanners(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<LootBoxBanner> banners = await context.LootBoxBanners
                .AsNoTracking()
                .Where(x => x.BoxId == id)
                .ToListAsync();

            return ResponseUtil.Ok(banners);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxDto lootBox)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.LootBoxes.AddAsync(lootBox.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(lootBox.Convert());
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPost("inventory")]
        public async Task<IActionResult> CreateInventory(LootBoxInventoryDto inventory)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.LootBoxInventories.AddAsync(inventory.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(inventory);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPost("banners")]
        public async Task<IActionResult> CreateBanner(LootBoxBannerDto banner)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.LootBoxBanners.AddAsync(banner.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(banner);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(LootBoxDto lootBox)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? oldBox = await context.LootBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == lootBox.Id);

            if (oldBox is null)
            {
                return ResponseUtil.NotFound(nameof(LootBox));
            }    

            try
            {
                context.Entry(oldBox).CurrentValues.SetValues(lootBox.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(lootBox);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPut("banners")]
        public async Task<IActionResult> UpdateBanner(LootBoxBannerDto banner)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner? oldBox = await context.LootBoxBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == banner.Id);

            if (oldBox is null)
            {
                return ResponseUtil.NotFound(nameof(LootBoxBanner));
            }

            try
            {
                context.Entry(oldBox).CurrentValues.SetValues(banner.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(banner);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? lootBox = await context.LootBoxes
                .AsNoTracking()
                .Include(x => x.Game)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));

            context.LootBoxes.Remove(lootBox);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(lootBox);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("banners/{id}")]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner? banner = await context.LootBoxBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (banner is null)
                return ResponseUtil.NotFound(nameof(LootBoxBanner));

            context.LootBoxBanners.Remove(banner);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(banner);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("inventory/{id}")]
        public async Task<IActionResult> DeleteItemFromInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxInventory? inventory = await context.LootBoxInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (inventory is null)
                return ResponseUtil.NotFound(nameof(LootBoxInventory));

            context.LootBoxInventories.Remove(inventory);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(inventory);
        }

    }
}
