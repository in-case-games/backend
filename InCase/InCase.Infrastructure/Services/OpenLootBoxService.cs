using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace InCase.Infrastructure.Services
{
    public class OpenLootBoxService
    {
        private const decimal RevenuePrecentage = 0.10M;
        private const decimal RetentionPrecentageBanner = 0.20M;
        private static readonly Random _random = new();

        public static void CheckNegativeBalance(ref LootBox box, ref SiteStatisticsAdmin statisticsAdmin) {
            if (box.Balance < 0)
            {
                //Notify admin negative balance
                statisticsAdmin.BalanceWithdrawn += box.Balance;
                box.Balance = 0;
            }
        }

        public static decimal GetDiscountAndActivatePromocode(UserHistoryPromocode? history, 
            ApplicationDbContext context)
        {
            decimal discount = 0;

            if (history is not null)
            {
                discount = history.Promocode!.Discount;

                history.IsActivated = true;
                history.Date = DateTime.UtcNow;

                context.UserHistoryPromocodes.Attach(history);
                context.Entry(history).Property(p => p.IsActivated).IsModified = true;
                context.Entry(history).Property(p => p.Date).IsModified = true;
            }

            return discount;
        }

        public async static Task<UserPathBanner?> GetPathBanner(
            Guid userId,
            LootBoxBanner? banner, 
            ApplicationDbContext context)
        {
            UserPathBanner? pathBanner = null;

            if (banner?.Id is not null)
            {
                pathBanner = await context.UserPathBanners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(upb => upb.BannerId == banner.Id && upb.UserId == userId);
            }

            return pathBanner;
        }

        public static void CheckCashBackAndRevenue(
           ref decimal revenue,
           ref UserPathBanner path,
           ref UserAdditionalInfo info,
           decimal cashBack,
           ApplicationDbContext context)
        {
            if (cashBack >= 0)
            {
                info.Balance += cashBack * (1M - RevenuePrecentage);
                revenue = cashBack * RevenuePrecentage;

                context.UserPathBanners.Remove(path);
            }
            else
            {
                revenue = 0;
                context.UserPathBanners.Attach(path);
                context.Entry(path).Property(p => p.NumberSteps).IsModified = true;
            }
        }

        public static decimal GetRetentionAmount(decimal boxCost) => boxCost * RetentionPrecentageBanner;
        public static decimal GetRevenue(decimal boxCost) => boxCost * RevenuePrecentage;
        public static decimal GetExpenses(decimal winItemCost, decimal revenue) => winItemCost + revenue;

        public static GameItem RandomizeBySmallest(in LootBox box, bool IsVirtual = false)
        {
            List<int> chances = box.Inventories!
                .Select(s => s.ChanceWining)
                .ToList();

            int index = Randomizer(chances);
            GameItem item = box.Inventories![index].Item!;

            if (IsProfitCase(item, box, IsVirtual) is false)
            {
                List<GameItem> items = box.Inventories
                    .Select(s => s.Item)
                    .ToList()!;

                item = items.MinBy(m => m.Cost)!;
            }

            return item;
        }

        public static decimal GetCashBack(
            Guid itemGuid,
            decimal boxCost,
            UserPathBanner pathBanner)
        {
            decimal retention = boxCost * RetentionPrecentageBanner;
            decimal exact = pathBanner.FixedCost / retention;
            decimal ceiling = Math.Ceiling(exact);

            if (pathBanner.NumberSteps == 0)
                return (ceiling - exact) * retention;
            else if (pathBanner.ItemId == itemGuid)
                return (ceiling - pathBanner.NumberSteps) * retention;
            else
                return -1M;
        }

        public static void CheckWinItemAndExpenses(
            ref GameItem winItem,
            ref decimal expenses,
            in LootBox box,
            UserPathBanner pathBanner)
        {
            decimal retention = box.Cost * RetentionPrecentageBanner;
            expenses = winItem.Cost + retention;

            if (pathBanner.NumberSteps == 0)
            {
                winItem = box.Inventories!
                    .FirstOrDefault(f => f.ItemId == pathBanner.ItemId)!.Item!;
                winItem.Cost = pathBanner.FixedCost;

                expenses = retention;
            }
        }

        private static bool IsProfitCase(GameItem item, LootBox box, bool IsVirtual = false)
        {
            decimal balance = IsVirtual ? box.VirtualBalance : box.Balance;
            decimal revenue = balance * RevenuePrecentage;
            decimal availableBalance = balance - revenue;

            return item.Cost <= availableBalance;
        }

        private static int Randomizer(List<int> chances)
        {
            List<List<int>> partsChances = new();
            int start = 0;
            int length;
            int index = 0;

            for (int i = 0; i < chances.Count; i++)
            {
                length = chances[i];
                partsChances.Add(new List<int>() { start, start + length - 1 });
                start += length;
            }

            int maxValue = partsChances[^1][1];
            int random = _random.Next(0, maxValue + 1);

            for (int i = 0; i < partsChances.Count; i++)
            {
                List<int> part = partsChances[i];
                if (part[0] <= random && part[1] >= random)
                {
                    index = i;
                }
            }

            return index;
        }
    }
}
