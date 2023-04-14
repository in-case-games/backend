using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
                .Include(x => x.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == UserId);

            return (user is null) ? 
                ResponseUtil.NotFound(nameof(User)) : 
                ResponseUtil.Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.Users
                .Include(x => x.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (user is null)
                return ResponseUtil.NotFound(nameof(User));

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

            return ResponseUtil.Ok(promocodes);
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

            return ResponseUtil.Ok(withdrawns);
        }

        [AllowAnonymous]
        [HttpGet("{id}/history/withdrawns")]
        public async Task<IActionResult> GetWithdrawnsById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(a => a.Id == id))
                return ResponseUtil.NotFound(nameof(User));

            List<UserHistoryWithdrawn> withdrawns = await context.UserHistoryWithdrawns
                .AsNoTracking()
                .Include(i => i.Item)
                .Where(w => w.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(withdrawns);
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

            return ResponseUtil.Ok(openings);
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

            return ResponseUtil.Ok(banners);
        }

        [AllowAnonymous]
        [HttpGet("{id}/inventory")]
        public async Task<IActionResult> GetInventoryByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(a => a.Id == id))
                return ResponseUtil.NotFound(nameof(User));

            List<UserInventory> inventories = await context.UserInventories
                .Include(i => i.Item)
                .AsNoTracking()
                .Where(w => w.UserId == id)
                .ToListAsync();

            return ResponseUtil.Ok(inventories);
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

            return ResponseUtil.Ok(inventories);
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

            return ResponseUtil.Ok(payments);
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
                .FirstOrDefaultAsync();

            if (item is null)
                return ResponseUtil.NotFound(nameof(GameItem));
            if (banner is null)
                return ResponseUtil.NotFound(nameof(LootBoxBanner));
            if(await context.UserPathBanners.AnyAsync(a => a.UserId == UserId && a.BannerId == banner.Id))
                return ResponseUtil.Conflict("User path banner is exist");

            LootBox box = banner.Box!;

            if (item.Cost <= box.Cost)
                return ResponseUtil.Conflict("The cost of the item cannot be less than the cost of the case");

            pathDto.UserId = UserId;
            pathDto.Date = DateTime.UtcNow;
            pathDto.NumberSteps = (int)Math.Ceiling(item.Cost/(box.Cost * 0.2M));
            pathDto.FixedCost = item.Cost;

            return await EndpointUtil.Create(pathDto.Convert(), context);
        }

        // TODO Transfer method
        [AuthorizeRoles(Roles.All)]
        [HttpDelete("banner/{id}")]
        public async Task<IActionResult> RemovePathBanner(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserPathBanner? pathBanner = await context.UserPathBanners
                .Include(i => i.Banner)
                .Include(i => i.Banner!.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.BannerId == id && f.UserId == UserId);
            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(f => f.UserId == UserId);

            if (userInfo is null)
                return ResponseUtil.NotFound(nameof(UserAdditionalInfo));
            if (pathBanner is null)
                return ResponseUtil.NotFound(nameof(UserPathBanner));

            SiteStatisticsAdmin statisticsAdmin = await context.SiteStatisticsAdmins
                .FirstAsync();

            decimal totalSpent = pathBanner.NumberSteps * pathBanner.Banner!.Box!.Cost;

            statisticsAdmin.BalanceWithdrawn += totalSpent * 0.1M;
            userInfo.Balance += totalSpent * 0.9M;

            return await EndpointUtil.Delete(pathBanner, context);
        }
    }
}
