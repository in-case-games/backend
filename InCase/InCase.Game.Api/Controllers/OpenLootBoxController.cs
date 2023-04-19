﻿using InCase.Domain.Common;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
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
        private const decimal RevenuePrecentage = 0.10M;
        private const decimal RetentionPrecentageBanner = 0.20M;
        private static readonly Random _random = new();
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public OpenLootBoxController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOpeningLootBox(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            LootBox? lootBox = await context.LootBoxes
                .Include(i => i.Inventories!)
                    .ThenInclude(ti => ti!.Item)
                .Include(i => i.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            UserPathBanner? pathBanner = null;

            if (lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));

            User? user = await context.Users
                .Include(i => i.AdditionalInfo)
                .Include(i => i.HistoryPromocodes!)
                    .ThenInclude(ti => ti.Promocode)
                        .ThenInclude(ti => ti!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == UserId);

            if(user is null || user.AdditionalInfo is null)
                return ResponseUtil.NotFound("User");

            UserAdditionalInfo userInfo = user.AdditionalInfo;
            UserHistoryPromocode? promocode = user.HistoryPromocodes?
                .FirstOrDefault(f => f.IsActivated == false && f.Promocode?.Type?.Name == "case");

            if (lootBox.IsLocked)
                return ResponseUtil.Conflict("Loot box is locked");
            if (userInfo.Balance < lootBox.Cost) 
                return ResponseUtil.Conflict("Insufficient funds");
            if (lootBox.Banner?.Id is not null)
            {
                pathBanner = await context.UserPathBanners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.BannerId == lootBox.Banner!.Id && f.UserId == UserId);
            }

            decimal discount = 0;

            if (promocode is not null)
            {
                discount = promocode.Promocode!.Discount;

                promocode.IsActivated = true;
                promocode.Date = DateTime.UtcNow;

                context.UserHistoryPromocodes.Attach(promocode);
                context.Entry(promocode).Property(p => p.IsActivated).IsModified = true;
                context.Entry(promocode).Property(p => p.Date).IsModified = true;
            }

            decimal lootBoxCost = discount >= 0.99M ? 1 : lootBox.Cost * (1M - discount);

            //Update Balance Case and User
            userInfo.Balance -= lootBoxCost;
            lootBox.Balance += lootBoxCost;

            //Calling random
            GameItem winItem = RandomizeBySmallest(in lootBox);
            SiteStatisticsAdmin statisticsAdmin = await context.SiteStatisticsAdmins
                .FirstAsync();
            SiteStatistics statistics = await context.SiteStatistics
                .FirstAsync();

            decimal revenue = lootBox.Cost * RevenuePrecentage;
            decimal expenses = winItem.Cost + revenue;

            if (pathBanner is not null && lootBox.Banner!.IsActive == true)
            {
                --pathBanner.NumberSteps;

                decimal retentionAmount = lootBox.Cost * RetentionPrecentageBanner;
                decimal cashBack = GetCashBack(winItem.Id, lootBox.Cost, pathBanner);

                CheckWinItemAndExpenses(ref winItem, ref expenses, lootBox, pathBanner);

                if (cashBack >= 0)
                {
                    userInfo.Balance += cashBack * (1M - RevenuePrecentage);
                    revenue = cashBack * RevenuePrecentage;

                    context.UserPathBanners.Remove(pathBanner);
                }
                else
                {
                    revenue = 0;
                    context.UserPathBanners.Attach(pathBanner);
                    context.Entry(pathBanner).Property(p => p.NumberSteps).IsModified = true;
                }
            }

            statistics.LootBoxes++;

            statisticsAdmin.BalanceWithdrawn += revenue;
            lootBox.Balance -= expenses;

            if (lootBox.Balance < 0)
            {
                //Notify admin negative balance
                statisticsAdmin.BalanceWithdrawn += lootBox.Balance;
                lootBox.Balance = 0;
            }

            context.LootBoxes.Attach(lootBox);
            context.UserAdditionalInfos.Attach(userInfo);
            context.Entry(userInfo).Property(p => p.Balance).IsModified = true;
            context.Entry(lootBox).Property(p => p.Balance).IsModified = true;

            DateTime date = DateTime.UtcNow; 

            //Add history and add inventory user
            UserHistoryOpening history = new()
            {
                Id = new Guid(),
                UserId = UserId,
                BoxId = lootBox.Id,
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

            await context.UserHistoryOpenings.AddAsync(history);
            await context.UserInventories.AddAsync(inventory);

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(winItem);
        }

        [AuthorizeRoles(Roles.All)]
        [HttpGet("virtual/{id}")]
        public async Task<IActionResult> GetVirtualOpeningLootBox(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? userInfo = await context.UserAdditionalInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UserId == UserId);
            LootBox? lootBox = await context.LootBoxes
                .Include(i => i.Inventories!)
                    .ThenInclude(ti => ti!.Item)
                .Include(i => i.Banner)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (userInfo is null || lootBox is null)
                return ResponseUtil.NotFound(nameof(LootBox));
            if (!userInfo.IsGuestMode)
                return Forbid();

            //Update Balance Case and User
            lootBox.VirtualBalance += lootBox.Cost;

            //Calling random
            GameItem winItem = RandomizeBySmallest(in lootBox, true);

            decimal revenue = lootBox.Cost * RevenuePrecentage;
            decimal expensesCase = winItem.Cost + revenue;

            lootBox.VirtualBalance -= expensesCase;

            context.LootBoxes.Attach(lootBox);
            context.Entry(lootBox).Property(p => p.VirtualBalance).IsModified = true;

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(winItem);
        }

        // TODO: Rebase this
        #region nonAction
        private static GameItem RandomizeBySmallest(in LootBox box, bool IsVirtual = false)
        {
            List<int> chances = box.Inventories!
                .Select(x => x.ChanceWining)
                .ToList();

            int winIndex = Randomizer(chances);
            GameItem winItem = box.Inventories![winIndex].Item!;

            if (IsProfitCase(winItem, box, IsVirtual) is false)
            {
                List<GameItem> items = box.Inventories
                    .Select(x => x.Item)
                    .ToList()!;

                winItem = items.MinBy(x => x.Cost)!;
            }

            return winItem;
        }
        private static decimal GetCashBack(
            Guid itemGuid, 
            decimal boxCost, 
            UserPathBanner pathBanner)
        {
            decimal retentionAmount = boxCost * RetentionPrecentageBanner;
            decimal exactSteps = pathBanner.FixedCost / retentionAmount;
            decimal ceilingSteps = Math.Ceiling(exactSteps);

            if (pathBanner.NumberSteps == 0)
                return (ceilingSteps - exactSteps) * retentionAmount;
            else if (pathBanner.ItemId == itemGuid)
                return (ceilingSteps - pathBanner.NumberSteps) * retentionAmount;
            else
                return -1M;
        }

        private static void CheckWinItemAndExpenses(
            ref GameItem winItem, 
            ref decimal expenses,
            in LootBox box,
            UserPathBanner pathBanner)
        {
            decimal retentionAmount = box.Cost * RetentionPrecentageBanner;
            expenses = winItem.Cost + retentionAmount;

            if (pathBanner.NumberSteps == 0)
            {
                winItem = box.Inventories!
                    .FirstOrDefault(f => f.ItemId == pathBanner.ItemId)!.Item!;
                winItem.Cost = pathBanner.FixedCost;

                expenses = retentionAmount;
            }
        }

        private static bool IsProfitCase(GameItem item, LootBox box, bool IsVirtual = false)
        {
            decimal boxBalance = IsVirtual ? box.VirtualBalance : box.Balance;
            decimal revenue = boxBalance * RevenuePrecentage;
            decimal availableBalance = boxBalance - revenue;

            return item.Cost <= availableBalance;
        }

        private static int Randomizer(List<int> chances)
        {
            List<List<int>> partsChances = new();
            int startParts = 0;
            int lengthPart;
            int winIndex = 0;

            for (int i = 0; i < chances.Count; i++)
            {
                lengthPart = chances[i];
                partsChances.Add(new List<int>() { startParts, startParts + lengthPart - 1 });
                startParts += lengthPart;
            }

            int maxRandomValue = partsChances[^1][1];
            int randomNumber = _random.Next(0, maxRandomValue + 1);

            for (int i = 0; i < partsChances.Count; i++)
            {
                List<int> part = partsChances[i];
                if (part[0] <= randomNumber && part[1] >= randomNumber)
                {
                    winIndex = i;
                }
            }

            return winIndex;
        }
        #endregion
    }
}
