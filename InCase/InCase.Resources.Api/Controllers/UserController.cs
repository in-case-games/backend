using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Payment;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
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
        private readonly WithdrawItemService _withdrawService;

        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(IDbContextFactory<ApplicationDbContext> contextFactory, 
            WithdrawItemService withdrawService)
        {
            _contextFactory = contextFactory;
            _withdrawService = withdrawService;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == UserId) ??
                throw new NotFoundCodeException("Пользователь не найден");

            return ResponseUtil.Ok(user.Convert(false));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id) ??
                throw new NotFoundCodeException("Пользователь не найден");

            return ResponseUtil.Ok(user.Convert(false));
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/promocodes")]
        public async Task<IActionResult> GetPromocodes()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryPromocode> promocodes = await context.UserHistoryPromocodes
                .Include(uhp => uhp.Promocode)
                .AsNoTracking()
                .Where(uhp => uhp.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(promocodes);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/withdraws")]
        public async Task<IActionResult> GetWithdraws()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryWithdraw> withdrawns = await context.UserHistoryWithdraws
                .AsNoTracking()
                .Include(uhw => uhw.Item)
                .Where(uhw => uhw.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(withdrawns);
        }

        [AllowAnonymous]
        [HttpGet("{id}/history/withdraws")]
        public async Task<IActionResult> GetWithdrawsById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Users.AnyAsync(u => u.Id == id))
                throw new NotFoundCodeException("Пользователь не найден");

            List<UserHistoryWithdraw> withdraws = await context.UserHistoryWithdraws
                .Include(uhw => uhw.Item)
                .AsNoTracking()
                .Where(uhw => uhw.UserId == id)
                .OrderByDescending(uhw => uhw.Date)
                .Take(100)
                .ToListAsync();

            return ResponseUtil.Ok(withdraws);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("history/openings")]
        public async Task<IActionResult> GetOpenings()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpening> openings = await context.UserHistoryOpenings
                .Include(uho => uho.Box)
                .Include(uho => uho.Item)
                .AsNoTracking()
                .Where(uho => uho.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(openings);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("banners")]
        public async Task<IActionResult> GetPathBanners()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserPathBanner> banners = await context.UserPathBanners
                .Include(upb => upb.Banner)
                .Include(upb => upb.Item)
                .AsNoTracking()
                .Where(upb => upb.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(banners);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("banner/{id}")]
        public async Task<IActionResult> GetPathBanners(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (!await context.LootBoxBanners.AnyAsync(lbb => lbb.Id == id))
                throw new NotFoundCodeException("Баннер не найден");

            UserPathBanner banner = await context.UserPathBanners
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BannerId == id && upb.UserId == UserId) ??
                throw new NotFoundCodeException("Путь к баннеру не найден");

            return ResponseUtil.Ok(banner.Convert(false));
        }

        [AllowAnonymous]
        [HttpGet("{id}/inventory")]
        public async Task<IActionResult> GetInventoryByUserId(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if(!await context.Users.AnyAsync(u => u.Id == id))
                throw new NotFoundCodeException("Пользователь не найден");

            List<UserInventory> inventories = await context.UserInventories
                .Include(ui => ui.Item)
                .AsNoTracking()
                .Where(ui => ui.UserId == id)
                .OrderByDescending(ui => ui.Date)
                .Take(100)
                .ToListAsync();

            return ResponseUtil.Ok(inventories);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventories
                .Include(ui => ui.Item)
                .AsNoTracking()
                .Where(ui => ui.UserId == UserId)
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
                .Where(uhp => uhp.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(payments);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("activate/promocode/{name}")]
        public async Task<IActionResult> ActivatePromocode(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode promocode = await context.Promocodes
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundCodeException("Промокод не найден");

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ForbiddenCodeException("Промокод истёк");

            UserHistoryPromocode? historyPromocode = await context.UserHistoryPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.PromocodeId == promocode.Id);

            UserHistoryPromocode? historyPromocodeType = await context.UserHistoryPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => 
                uhp.Promocode!.Type!.Id == promocode.TypeId && 
                uhp.IsActivated == false && 
                uhp.UserId == UserId);

            if (historyPromocode is not null && historyPromocode.IsActivated)
                throw new ConflictCodeException("Промокод уже используется");
            if (historyPromocodeType is not null)
                throw new ConflictCodeException("Тип промокода уже используется");

            promocode.NumberActivations--;

            historyPromocode = new() { 
                IsActivated = false,
                PromocodeId = promocode.Id,
                UserId = UserId
            };

            return await EndpointUtil.Create(historyPromocode, context);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("exchange/promocode/{name}")]
        public async Task<IActionResult> ExchangePromocode(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode promocode = await context.Promocodes
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundCodeException("Промокод не найден");

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ConflictCodeException("Промокод истёк");

            bool IsUsed = await context.UserHistoryPromocodes
                .AnyAsync(uhp => uhp.PromocodeId == promocode.Id && uhp.IsActivated);

            if (IsUsed)
                throw new ConflictCodeException("Промокод уже использован");

            UserHistoryPromocode? promocodeOld = await context.UserHistoryPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .FirstOrDefaultAsync(uhp => 
                uhp.Promocode!.Type!.Id == promocode.TypeId && 
                uhp.IsActivated == false &&
                uhp.UserId == UserId);

            if (promocodeOld is null)
                throw new ConflictCodeException("Прошлый промокод не найден");
            if (promocodeOld.Promocode!.Id == promocode.Id)
                throw new ConflictCodeException("Промокод уже использован");

            promocodeOld.Promocode.NumberActivations++;
            promocode.NumberActivations--;

            UserHistoryPromocode? promocodeNew = new()
            {
                Id = promocodeOld.Id,
                UserId = UserId,
                PromocodeId = promocode.Id,
                IsActivated = false,
                Date = null
            };

            return await EndpointUtil.Update(promocodeOld, promocodeNew, context);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory/sell/{id}")]
        public async Task<IActionResult> SellLastOpeningGameItem(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == UserId) ??
                throw new NotFoundCodeException("Дополнительная информация не найдена");

            List<UserInventory> inventories = await context.UserInventories
                .AsNoTracking()
                .Where(ui => ui.UserId == UserId && ui.ItemId == id)
                .ToListAsync();

            if (inventories.Count == 0)
                throw new ConflictCodeException("Инвентарь пуст");

            UserInventory inventory = inventories.MinBy(ui => ui.Date)!;

            info.Balance += inventory.FixedCost;

            return await EndpointUtil.Delete(inventory, context);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory/{id}/sell")]
        public async Task<IActionResult> SellGameItemByInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == UserId) ??
                throw new NotFoundCodeException("Дополнительная информация не найдена");

            UserInventory inventory = await context.UserInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id && ui.UserId == UserId) ??
                throw new NotFoundCodeException("Предмет не найден в инвентаре");

            info.Balance += inventory.FixedCost;

            return await EndpointUtil.Delete(inventory, context);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("inventory/{id}/exchange/{itemId}")]
        public async Task<IActionResult> ExchangeGameItem(Guid id, Guid itemId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory inventory = await context.UserInventories
                .Include(ui => ui.Item)
                .Include(ui => ui.Item!.Game!)
                    .ThenInclude(g => g.Markets)
                .FirstOrDefaultAsync(ui => ui.UserId == UserId && ui.Id == id) ??
                throw new NotFoundCodeException("Предмет не найден в инвентаре");
            GameItem item = await context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == itemId) ??
                throw new NotFoundCodeException("Предмет не найден");
            UserAdditionalInfo info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == UserId) ??
                throw new NotFoundCodeException("Дополнительная информация не найдена");

            decimal differenceCost = inventory.FixedCost - item.Cost;

            if (differenceCost < 0)
                throw new BadRequestCodeException("Стоимость товара при обмене не может быть выше");

            ItemInfo itemInfo = await _withdrawService.GetItemInfo(inventory.Item!);

            decimal itemInfoPrice = itemInfo.PriceKopecks * 0.01M;

            if (itemInfoPrice <= inventory.FixedCost * 0.1M / 7)
                throw new ConflictCodeException("Товар может быть обменен только в случае нестабильности цены");

            inventory.ItemId = item.Id;
            inventory.FixedCost = item.Cost;
            info.Balance += differenceCost;

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(inventory);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpPost("banner")]
        public async Task<IActionResult> CreatePathBanner(UserPathBannerDto pathDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBoxBanner banner = await context.LootBoxBanners
                .Include(lbb => lbb.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbb => lbb.Id == pathDto.BannerId) ?? 
                throw new NotFoundCodeException("Баннер не найден");

            LootBoxInventory inventory = await context.LootBoxInventories
                .Include(lbi => lbi.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(lbi => lbi.ItemId == pathDto.ItemId && lbi.BoxId == banner.BoxId) ??
                throw new NotFoundCodeException("Предмет не найден в кейсе");

            if (banner.IsActive is false || banner.ExpirationDate < DateTime.UtcNow)
                throw new ForbiddenCodeException("Баннер не активен");
            if (await context.UserPathBanners.AnyAsync(upb => upb.UserId == UserId && upb.BannerId == banner.Id))
                throw new ConflictCodeException("Путь к баннеру уже используется");

            GameItem item = inventory.Item!;
            LootBox box = banner.Box!;

            if (item.Cost <= box.Cost)
                throw new BadRequestCodeException("Стоимость товара не может быть меньше стоимости кейса");

            pathDto.UserId = UserId;
            pathDto.Date = DateTime.UtcNow;
            pathDto.NumberSteps = (int)Math.Ceiling(item.Cost/(box.Cost * 0.2M));
            pathDto.FixedCost = item.Cost;

            return pathDto.NumberSteps <= 100 ? 
                await EndpointUtil.Create(pathDto.Convert(), context) :
                throw new BadRequestCodeException("Стоимость предмета превышает стоимость кейса в 20 раз");
        }

        [AuthorizeRoles(Roles.All)]
        [HttpDelete("transfer/withdraw/{id}/inventory")]
        public async Task<IActionResult> TransferWithdrawToInventory(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryWithdraw withdraw = await context.UserHistoryWithdraws
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhw => uhw.Id == id && uhw.UserId == UserId) ??
                throw new NotFoundCodeException("История вывода не найдена");

            if (withdraw.Status?.Name is null || withdraw.Status.Name != "cancel")
                throw new ConflictCodeException("Ваш предмет выводится");

            UserInventory inventory = new()
            {
                Date = withdraw.Date,
                FixedCost = withdraw.FixedCost,
                ItemId = withdraw.ItemId,
                UserId = UserId
            };

            await context.UserInventories.AddAsync(inventory);

            return await EndpointUtil.Delete(withdraw, context);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpDelete("banner/{id}")]
        public async Task<IActionResult> RemovePathBanner(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserPathBanner path = await context.UserPathBanners
                .Include(upb => upb.Banner)
                .Include(upb => upb.Banner!.Box)
                .AsNoTracking()
                .FirstOrDefaultAsync(upb => upb.BannerId == id && upb.UserId == UserId) ??
                throw new NotFoundCodeException("Путь к баннеру не найден");
            UserAdditionalInfo info = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(uai => uai.UserId == UserId) ??
                throw new NotFoundCodeException("Дополнительная информация не найдена");

            SiteStatisticsAdmin statistics = await context.SiteStatisticsAdmins
                .FirstAsync();

            decimal totalSpent = path.NumberSteps * path.Banner!.Box!.Cost;

            statistics.BalanceWithdrawn += totalSpent * 0.1M;
            info.Balance += totalSpent * 0.9M;

            await EndpointUtil.Delete(path, context);

            return ResponseUtil.Ok(path.Convert(false));
        }

        [AllowAnonymous]
        [HttpGet("history/withdraws/100")]
        public async Task<IActionResult> GetLast100Withdraws()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryWithdrawDto> withdraws = new();

            await context.UserHistoryWithdraws
                .AsNoTracking()
                .OrderByDescending(uhw => uhw.Date)
                .Take(100)
                .ForEachAsync(uhw => withdraws.Add(uhw.Convert(false)));

            return ResponseUtil.Ok(withdraws);
        }

        [AllowAnonymous]
        [HttpGet("history/openings/100")]
        public async Task<IActionResult> GetLast100Openings()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningDto> openings = new();

            await context.UserHistoryOpenings
                .AsNoTracking()
                .OrderByDescending(uho => uho.Date)
                .Take(100)
                .ForEachAsync(uhp => openings.Add(uhp.Convert(false)));

            return ResponseUtil.Ok(openings);
        }
    }
}
