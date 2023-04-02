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
            return await EndpointUtil.Create(lootBox.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPost("inventory")]
        public async Task<IActionResult> CreateInventory(LootBoxInventoryDto inventory)
        {
            return await EndpointUtil.Create(inventory.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPost("banners")]
        public async Task<IActionResult> CreateBanner(LootBoxBannerDto banner)
        {
            return await EndpointUtil.Create(banner.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPut]
        public async Task<IActionResult> Update(LootBoxDto lootBox)
        {
            return await EndpointUtil.Update(lootBox.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpPut("banners")]
        public async Task<IActionResult> UpdateBanner(LootBoxBannerDto banner)
        {
            return await EndpointUtil.Update(banner.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<LootBox>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("banners/{id}")]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            return await EndpointUtil.Delete<LootBoxBanner>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Bot, Roles.Owner)]
        [HttpDelete("inventory/{id}")]
        public async Task<IActionResult> DeleteItemFromInventory(Guid id)
        {
            return await EndpointUtil.Delete<LootBoxInventory>(id, _contextFactory);
        }

    }
}
