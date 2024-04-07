using Game.BLL.Constants;
using Game.DAL.Entities;

namespace Game.BLL.Services;
public static class OpenLootBoxService
{
	private static readonly Random Random = new();

	public static GameItem RandomizeBySmallest(in LootBox box, bool isPlayBanner, bool isVirtual = false)
	{
		var index = Randomize(box.Inventories!.Select(s => s.ChanceWining));
		var item = box.Inventories!.ToList()[index].Item!;

		if (IsProfitCase(item, box, isPlayBanner, isVirtual)) return item;
		
		return box.Inventories!.Select(s => s.Item).MinBy(m => m!.Cost)!;
	}

	/*
		Неучтенный момент с тем, что стоимость кейса может быть ниже при промокоде и может фальшивить revenue, 
		который зависит от стоимости кейса динамически. 
		Пример: 
			Кейс стоит - 400 
			При промокоде будет - 300
			А здесь все равно учет идет как 400 на 38, 39 строке
	*/
	private static bool IsProfitCase(GameItem item, LootBox box, bool isPlayBanner, bool isVirtual = false)
	{
		var balance = isVirtual ? box.VirtualBalance : box.Balance;
		var revenue = box.Cost * CommonConstants.RevenuePercentage;
		var availableBalance = balance - revenue;

		if(isPlayBanner) 
		{
			var revenueBanner = balance * CommonConstants.RevenuePercentageBanner;
			var retentionBanner = box.Cost * CommonConstants.RetentionPercentageBanner;

			availableBalance = balance - retentionBanner - revenueBanner;
		}

		return item.Cost <= availableBalance;
	}

	private static int Randomize(IEnumerable<int> chances)
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