using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Game.Api.Controllers
{
    [Route("api/open-loot-box")]
    [ApiController]
    public class OpenLootBoxController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public OpenLootBoxController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOpeningLootBox(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            LootBox box = await context.LootBoxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Кейс не найден");
            UserAdditionalInfo userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Пользователь не найден");

            UserHistoryPromocode? promocode = await context.UserHistoryPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .FirstOrDefaultAsync(uhp => 
                uhp.UserId == UserId &&
                !uhp.IsActivated && 
                uhp.Promocode!.Type!.Name == "case", cancellationToken);

            if (box.IsLocked)
                throw new ForbiddenCodeException("Кейс заблокирован");
            if (userInfo.Balance < box.Cost)
                throw new PaymentRequiredCodeException("Недостаточно средств");

            UserPathBanner? pathBanner = await OpenLootBoxService.GetPathBanner(UserId, box.Banner, context);
            decimal discount = OpenLootBoxService.GetDiscountAndActivatePromocode(promocode, context);
            decimal boxCost = discount >= 0.99M ? 1 : box.Cost * (1M - discount);

            userInfo.Balance -= boxCost;
            box.Balance += boxCost;

            GameItem winItem = OpenLootBoxService.RandomizeBySmallest(in box);
            SiteStatisticsAdmin statisticsAdmin = await context.SiteStatisticsAdmins
                .FirstAsync(cancellationToken);
            SiteStatistics statistics = await context.SiteStatistics
                .FirstAsync(cancellationToken);

            decimal revenue = OpenLootBoxService.GetRevenue(box.Cost);
            decimal expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

            if (ValidationService.IsActiveBanner(in pathBanner!, in box))
            {
                --pathBanner!.NumberSteps;

                decimal retentionAmount = OpenLootBoxService.GetRetentionAmount(box.Cost);
                decimal cashBack = OpenLootBoxService.GetCashBack(winItem.Id, box.Cost, pathBanner);

                OpenLootBoxService.CheckWinItemAndExpenses(ref winItem, ref expenses, box, pathBanner);
                OpenLootBoxService.CheckCashBackAndRevenue(
                    ref revenue, ref pathBanner, ref userInfo, 
                    cashBack, context);
            }

            statistics.LootBoxes++;
            statisticsAdmin.BalanceWithdrawn += revenue;
            box.Balance -= expenses;

            OpenLootBoxService.CheckNegativeBalance(ref box, ref statisticsAdmin);

            context.LootBoxes.Attach(box);
            context.UserAdditionalInfos.Attach(userInfo);
            context.Entry(userInfo).Property(p => p.Balance).IsModified = true;
            context.Entry(box).Property(p => p.Balance).IsModified = true;

            DateTime date = DateTime.UtcNow; 

            UserHistoryOpening history = new()
            {
                Id = new Guid(),
                UserId = UserId,
                BoxId = box.Id,
                ItemId = winItem.Id,
                Date = date
            };
            UserInventory inventory = new()
            {
                Id = new Guid(),
                UserId = UserId,
                ItemId = winItem.Id,
                Date = date,
                FixedCost = winItem.Cost
            };

            await context.UserHistoryOpenings.AddAsync(history, cancellationToken);
            await context.UserInventories.AddAsync(inventory, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return ResponseUtil.Ok(winItem.Convert(false));
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("virtual/{id}")]
        public async Task<IActionResult> GetVirtualOpeningLootBox(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            UserAdditionalInfo userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(uai => uai.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Пользователь не найден");
            LootBox box = await context.LootBoxes
                .Include(lb => lb.Inventories!)
                    .ThenInclude(lbi => lbi.Item)
                .Include(lb => lb.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Кейс не найден");

            if (!userInfo.IsGuestMode)
                throw new ForbiddenCodeException("Не включен режим гостя");

            box.VirtualBalance += box.Cost;

            GameItem winItem = OpenLootBoxService.RandomizeBySmallest(in box, true);
            decimal revenue = OpenLootBoxService.GetRevenue(box.Cost);
            decimal expenses = OpenLootBoxService.GetExpenses(winItem.Cost, revenue);

            box.VirtualBalance -= expenses;

            context.LootBoxes.Attach(box);
            context.Entry(box).Property(p => p.VirtualBalance).IsModified = true;

            await context.SaveChangesAsync(cancellationToken);

            return ResponseUtil.Ok(winItem.Convert(false));
        }
    }
}
