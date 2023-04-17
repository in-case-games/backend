using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == UserId);

            return user is null ?
                ResponseUtil.NotFound("User") :
                ResponseUtil.Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (user is null)
                return ResponseUtil.NotFound("User");

            user.PasswordHash = null;
            user.PasswordSalt = null;

            return ResponseUtil.Ok(user);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/promocodes")]
        public async Task<IActionResult> GetPromocodes()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryPromocode> promocodes = await context.UserHistoryPromocodes
                .Include(i => i.Promocode)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return promocodes.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserHistoryPromocode)) :
                ResponseUtil.Ok(promocodes);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/withdrawns")]
        public async Task<IActionResult> GetWithdrawns()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryWithdrawn> withdrawns = await context.UserHistoryWithdrawns
                .AsNoTracking()
                .Include(i => i.Item)
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return withdrawns.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserHistoryWithdrawn)) :
                ResponseUtil.Ok(withdrawns);
        }

        [AllowAnonymous]
        [HttpGet("{id}/history/withdrawns")]
        public async Task<IActionResult> GetWithdrawnsById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(i => i.HistoryWithdrawns!)
                    .ThenInclude(ti => ti.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (user is null)
                return ResponseUtil.NotFound("User");

            return user.HistoryWithdrawns is null || user.HistoryWithdrawns.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserHistoryWithdrawn)) : 
                ResponseUtil.Ok(user.HistoryWithdrawns);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/openings")]
        public async Task<IActionResult> GetOpenings()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpening> openings = await context.UserHistoryOpenings
                .Include(i => i.Box)
                .Include(i => i.Item)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return openings.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserHistoryOpening)) :
                ResponseUtil.Ok(openings);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("banners")]
        public async Task<IActionResult> GetPathBanners()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserPathBanner> banners = await context.UserPathBanners
                .Include(i => i.Banner)
                .Include(i => i.Item)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return banners.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserPathBanner)) :
                ResponseUtil.Ok(banners);
        }

        [AllowAnonymous]
        [HttpGet("{id}/inventory")]
        public async Task<IActionResult> GetInventoryByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
               .Include(i => i.Inventories!)
                   .ThenInclude(ti => ti.Item)
               .AsNoTracking()
               .FirstOrDefaultAsync(f => f.Id == id);

            if (user is null)
                return ResponseUtil.NotFound("User");

            return user.Inventories is null || user.Inventories.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserInventory)) :
                ResponseUtil.Ok(user.Inventories);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventories
                .Include(i => i.Item)
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return inventories.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserInventory)) :
                ResponseUtil.Ok(inventories);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/payments")]
        public async Task<IActionResult> GetPayments()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryPayment> payments = await context.UserHistoryPayments
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return payments.Count == 0 ?
                ResponseUtil.NotFound(nameof(UserHistoryPayment)) :
                ResponseUtil.Ok(payments);
        }

        //TODO Transfer method
        [AuthorizeRoles(Roles.All)]
        [HttpGet("activate/promocode/{name}")]
        public async Task<IActionResult> ActivatePromocode(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .Include(i => i.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Name == name);

            if (promocode is null)
                return ResponseUtil.NotFound(nameof(Promocode));

            UserHistoryPromocode? historyPromocode = await context.UserHistoryPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.PromocodeId == promocode.Id);

            UserHistoryPromocode? historyPromocodeType = await context.UserHistoryPromocodes
                .Include(i => i.Promocode)
                .Include(i => i.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Promocode!.Type!.Id == promocode.TypeId && f.IsActivated == false && f.UserId == UserId);

            if (historyPromocode is not null && historyPromocode.IsActivated)
                return ResponseUtil.Conflict("Promocode has already been used");
            if (historyPromocodeType is not null)
                return ResponseUtil.Conflict("Promocode type is already in use");

            historyPromocode = new() { 
                IsActivated = false,
                PromocodeId = promocode.Id,
                UserId = UserId
            };

            return await EndpointUtil.Create(historyPromocode, context);
        }

        //TODO Transfer method
        [AuthorizeRoles(Roles.All)]
        [HttpGet("exchange/promocode/{name}")]
        public async Task<IActionResult> ExchangePromocode(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? promocode = await context.Promocodes
                .Include(i => i.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Name == name);

            if (promocode is null)
                return ResponseUtil.NotFound(nameof(Promocode));

            bool IsUsed = await context.UserHistoryPromocodes
                .AnyAsync(a => a.PromocodeId == promocode.Id && a.IsActivated == true);

            if (IsUsed)
                return ResponseUtil.Conflict("Promocode has already been used");

            UserHistoryPromocode? promocodeOld = await context.UserHistoryPromocodes
                .Include(i => i.Promocode)
                .Include(i => i.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Promocode!.Type!.Id == promocode.TypeId && f.IsActivated == false);

            if (promocodeOld is null)
                return ResponseUtil.Conflict("Promocode no exchange");

            UserHistoryPromocode? promocodeNew = new()
            {
                UserId = UserId,
                PromocodeId = promocode.Id
            };

            return await EndpointUtil.Update(promocodeOld, promocodeNew, context);
        }

        // TODO Transfer method
        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory/{id}/exchange/{itemId}")]
        public async Task<IActionResult> ExchangeGameItem(Guid id, Guid itemId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? inventory = await context.UserInventories
                .Include(i => i.Item)
                .FirstOrDefaultAsync(f => f.UserId == UserId && f.Id == id);
            GameItem? item = await context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == itemId);
            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(f => f.UserId == UserId);

            if (inventory is null)
                return ResponseUtil.NotFound(nameof(UserInventory));
            if (item is null)
                return ResponseUtil.NotFound(nameof(GameItem));
            if (info is null)
                return ResponseUtil.NotFound(nameof(UserAdditionalInfo));

            decimal differenceCost = inventory.Item!.Cost - item.Cost;

            if (differenceCost < 0)
                return ResponseUtil.Conflict("The value of the item in the exchange cannot be higher");

            inventory.ItemId = item.Id;
            info.Balance += differenceCost;

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(inventory);
        }

        // TODO Transfer method
        [AuthorizeRoles(Roles.All)]
        [HttpPost("banner")]
        public async Task<IActionResult> CreatePathBanner(UserPathBannerDto pathDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? item = await context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == pathDto.ItemId);
            LootBoxBanner? banner = await context.LootBoxBanners
                .Include(i => i.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == pathDto.BannerId);

            if (item is null)
                return ResponseUtil.NotFound(nameof(GameItem));
            if (banner is null)
                return ResponseUtil.NotFound(nameof(LootBoxBanner));
            if (await context.UserPathBanners.AnyAsync(a => a.UserId == UserId && a.BannerId == banner.Id))
                return ResponseUtil.Conflict("User path banner is exist");

            LootBox box = banner.Box!;

            if (item.Cost <= box.Cost)
                return ResponseUtil.Conflict("The cost of the item cannot be less than the cost of the case");

            pathDto.UserId = UserId;
            pathDto.Date = DateTime.UtcNow;
            pathDto.NumberSteps = (int)Math.Ceiling(item.Cost/(box.Cost * 0.2M));
            pathDto.FixedCost = item.Cost;

            if(pathDto.NumberSteps > 100)
                return ResponseUtil.Conflict("The cost of the selected item cannot exceed the loot box by more than 20 times");

            return await EndpointUtil.Create(pathDto.Convert(), context);
        }

        // TODO Transfer method
        [AuthorizeRoles(Roles.All)]
        [HttpDelete("banner/{id}")]
        public async Task<IActionResult> RemovePathBanner(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserPathBanner? path = await context.UserPathBanners
                .Include(i => i.Banner)
                .Include(i => i.Banner!.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.BannerId == id && f.UserId == UserId);
            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(f => f.UserId == UserId);

            if (info is null)
                return ResponseUtil.NotFound(nameof(UserAdditionalInfo));
            if (path is null)
                return ResponseUtil.NotFound(nameof(UserPathBanner));

            SiteStatisticsAdmin statisticsAdmin = await context.SiteStatisticsAdmins
                .FirstAsync();

            decimal totalSpent = path.NumberSteps * path.Banner!.Box!.Cost;

            statisticsAdmin.BalanceWithdrawn += totalSpent * 0.1M;
            info.Balance += totalSpent * 0.9M;

            return await EndpointUtil.Delete(path, context);
        }
    }
}
