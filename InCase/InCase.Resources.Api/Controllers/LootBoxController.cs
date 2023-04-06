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

            List<LootBox> lootBoxes = await context.LootBoxes
                .AsNoTracking()
                .Include(i => i.Game)
                .ToListAsync();

            return ResponseUtil.Ok(lootBoxes);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? lootBox = await context.LootBoxes
                .AsNoTracking()
                .Include(i => i.Game)
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

            List<LootBoxInventory> inventories = await context.LootBoxInventories
                .Include(i => i.Item)
                .AsNoTracking()
                .Where(w => w.BoxId == id)
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

            return ResponseUtil.Ok(banners);
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

            return banner is null ? 
                ResponseUtil.NotFound(nameof(LootBoxBanner)) : 
                ResponseUtil.Ok(banner);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Create(LootBoxDto lootBox)
        {
            return await EndpointUtil.Create(lootBox.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost("inventory")]
        public async Task<IActionResult> CreateInventory(LootBoxInventoryDto inventory)
        {
            return await EndpointUtil.Create(inventory.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPost("banner")]
        public async Task<IActionResult> CreateBanner(LootBoxBannerDto banner)
        {
            return await EndpointUtil.Create(banner.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Update(LootBoxDto lootBox)
        {
            return await EndpointUtil.Update(lootBox.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpPut("banner")]
        public async Task<IActionResult> UpdateBanner(LootBoxBannerDto banner)
        {
            return await EndpointUtil.Update(banner.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<LootBox>(id, _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("banner/{id}")]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            return await EndpointUtil.Delete<LootBoxBanner>(id, _contextFactory);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpDelete("inventory/{id}")]
        public async Task<IActionResult> DeleteItemFromInventory(Guid id)
        {
            return await EndpointUtil.Delete<LootBoxInventory>(id, _contextFactory);
        }
    }
}
