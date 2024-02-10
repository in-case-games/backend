using Game.DAL.Entities;

namespace Game.BLL.Services;
public static class OpenLootBoxService
{
    private static readonly Random Random = new();

    public const decimal RevenuePercentage = 0.10M;
    public const decimal RetentionPercentageBanner = 0.20M;

    public static decimal GetRevenue(decimal boxCost) => boxCost * RevenuePercentage;
    public static decimal GetExpenses(decimal winItemCost, decimal revenue) => winItemCost + revenue;
    public static decimal GetRetentionBanner(decimal boxCost) => boxCost * RetentionPercentageBanner;

    public static GameItem RandomizeBySmallest(in LootBox box, bool isVirtual = false)
    {
        var chances = box.Inventories!
            .Select(s => s.ChanceWining)
            .ToList();

        var index = Randomizer(chances);
        var item = box.Inventories!.ToList()[index].Item!;

        if (IsProfitCase(item, box, isVirtual)) return item;

        var items = box.Inventories!
            .Select(s => s.Item)
            .ToList()!;

        return items.MinBy(m => m!.Cost)!;
    }

    private static bool IsProfitCase(GameItem item, LootBox box, bool isVirtual = false)
    {
        var balance = isVirtual ? box.VirtualBalance : box.Balance;
        var revenue = balance * RevenuePercentage;
        var availableBalance = balance - revenue;

        return item.Cost <= availableBalance;
    }

    private static int Randomizer(IEnumerable<int> chances)
    {
        var partsChances = new List<List<int>>();
        var start = 0;
        var index = 0;

        foreach (var length in chances)
        {
            partsChances.Add([start, start + length - 1]);
            start += length;
        }

        var maxValue = partsChances[^1][1];
        var random = Random.Next(0, maxValue + 1);

        for (var i = 0; i < partsChances.Count; i++)
        {
            var part = partsChances[i];

            if (part[0] <= random && part[1] >= random) index = i;
        }

        return index;
    }
}