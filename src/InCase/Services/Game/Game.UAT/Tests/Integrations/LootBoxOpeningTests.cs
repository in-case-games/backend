using Game.BLL.Constants;
using Game.DAL.Entities;
using Game.UAT.Helpers;
using Game.UAT.Models;
using Game.UAT.TestData;
using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Xunit.Abstractions;

namespace Game.UAT.Tests.Integrations;
public class LootBoxOpeningTests(ITestOutputHelper outputHelper)
{
	private static readonly Random Random = new();

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_SmallPriceWinItem(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);
		var isHasItem = imageDb.Data.LootBoxInventories.Any(lbi => lbi.BoxId == lootBox.Id && lbi.ItemId == winItem.Id);

		//Assert
		Assert.True(isHasItem);
		Assert.True(winItem.Cost < lootBox.Cost);
	}

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_UserAndLootBoxBalance(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var userBalanceExpected = userInfo.Balance - lootBox.Cost;

		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var revenue = lootBox.Cost * CommonConstants.RevenuePercentage;
		var lootBoxBalanceExpected = lootBox.Cost - revenue - winItem.Cost;

		//Assert
		Assert.Equal(lootBoxBalanceExpected, lootBox.Balance);
		Assert.Equal(userBalanceExpected, userInfo.Balance);
	}

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_PublishInventoryTemplate(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
		
		var publishObjects = new List<object>();
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb, publishObjects);

		//Act
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var inventoryTemplate = new UserInventoryTemplate();

		foreach (var publishObject in publishObjects) 
		{
			if (publishObject is UserInventoryTemplate uit) 
			{
				inventoryTemplate = uit;
			}
		}

		//Assert
		Assert.Equal(winItem.Cost, inventoryTemplate.FixedCost);
		Assert.Equal(winItem.Id, inventoryTemplate.ItemId);
		Assert.Equal(userInfo.UserId, inventoryTemplate.UserId);
	}

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_PublishStatisticsTemplate(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
		
		var publishObjects = new List<object>();
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb, publishObjects);

		//Act
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var statisticsTemplate = new SiteStatisticsTemplate();

		foreach (var publishObject in publishObjects) 
		{
			if (publishObject is SiteStatisticsTemplate sst) 
			{
				statisticsTemplate = sst;
			}
		}

		//Assert
		Assert.Equal(1, statisticsTemplate.LootBoxes);
		Assert.Equal(0, statisticsTemplate.Reviews);
		Assert.Equal(0, statisticsTemplate.Users);
		Assert.Equal(0, statisticsTemplate.WithdrawnFunds);
		Assert.Equal(0, statisticsTemplate.WithdrawnItems);
	}

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_PublishStatisticsAdminTemplate(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
		
		var publishObjects = new List<object>();
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb, publishObjects);

		//Act
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var revenue = lootBox.Cost * CommonConstants.RevenuePercentage;

		var statisticsAdminTemplate = new SiteStatisticsAdminTemplate();

		foreach (var publishObject in publishObjects) 
		{
			if (publishObject is SiteStatisticsAdminTemplate sat)
			{
				statisticsAdminTemplate = sat;
			}
		}

		//Assert
		Assert.Equal(winItem.Cost, statisticsAdminTemplate.FundsUsersInventories);
		Assert.Equal(0, statisticsAdminTemplate.ReturnedFunds);
		Assert.Equal(revenue, statisticsAdminTemplate.RevenueLootBoxCommission);
		Assert.Equal(0, statisticsAdminTemplate.TotalReplenishedFunds);
	}
	
	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_PositiveBalance(ImageDb imageDb)
	{
		//Arrange
		var minFixBalance = 0M;
		var iterationTopLvl = Random.Next(100, 200);

		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		for(var i = 0; i < iterationTopLvl; i++) 
		{
			var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
			var iterationLowerLvl = Random.Next(1, 10);

			for(var j = 0; j < iterationLowerLvl; j++) 
			{
				var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();

				if(userInfos.Count <= 0) break;

				var userInfo = userInfos[Random.Next(0, userInfos.Count)];

				var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);
				
				var revenue = lootBox.Cost * CommonConstants.RevenuePercentage;

				minFixBalance = minFixBalance > lootBox.Balance ? lootBox.Balance : minFixBalance;
			}
		}

		//Assert
		Assert.Equal(0, minFixBalance);
	}

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_MoreOpeningsForCheckChances(ImageDb imageDb)
	{
		//Arrange
		var outputString = "";
		var revenue = 0M;
		var iterations = Random.Next(10000, 20000);
		var winIds = new List<Guid>(iterations);

		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];

		//Act
		for(var i = 0; i < iterations; i++) 
		{
			var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();

			if(userInfos.Count <= 0) break;

			var userInfo = userInfos[Random.Next(0, userInfos.Count)];
			var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

			winIds.Add(winItem.Id);
			
			revenue += lootBox.Cost * CommonConstants.RevenuePercentage;
		}

		foreach(var lootBoxInventory in lootBox.Inventories!) 
		{
			var expectedChance = Math.Round(lootBoxInventory.ChanceWining / 100000M, 2);
			var expectedCount = Math.Round(iterations * expectedChance / 100, 2);
			var actualCount = winIds.Count(id => id == lootBoxInventory.ItemId);
			var actualChance = Math.Round(actualCount * 100M / iterations, 2);
			outputString += $"Id - {lootBoxInventory.ItemId}: {Environment.NewLine}" + 
							$"Expected Count - {expectedCount}{Environment.NewLine}" + 
							$"Expected Chance - {expectedChance}%{Environment.NewLine}" +
							$"Actual Count - {actualCount}{Environment.NewLine}" +
							$"Actual Chance - {actualChance}%{Environment.NewLine}";
		}

		outputString += $"Loot Box Balance - {lootBox.Balance}{Environment.NewLine}";
		outputString += $"Revenue - {revenue}{Environment.NewLine}";

		//Assert
		Console.WriteLine(outputString);
		outputHelper.WriteLine(outputString);
		Assert.True(true);
	}

	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_SimpleData_MoreOpeningsForAccountingMoney(ImageDb imageDb)
	{
		//Arrange
		var minFixBalance = 0M;
		var revenue = 0M;
		var revenueExpected = 0M;
		var fundsUsersInventories = 0M;
		var fundsUsersInventoriesExpected = 0M;
		var openingCosts = 0M;
		var inventoryCost = 0M;
		var iterations = Random.Next(5000, 10000);
		var winIds = new List<Guid>(iterations);
		var publishObjects = new List<object>();
		var lootBoxInventoriesAndNumberDrops = new Dictionary<LootBoxInventory, int>();

		var openingService = MockHelper.FillLootBoxOpeningService(imageDb, publishObjects);
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];

		//Act
		for(var i = 0; i < iterations; i++) 
		{
			var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();

			if(userInfos.Count <= 0) break;

			var userInfo = userInfos[Random.Next(0, userInfos.Count)];
			var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

			winIds.Add(winItem.Id);
			
			revenueExpected += lootBox.Cost * CommonConstants.RevenuePercentage;
			fundsUsersInventoriesExpected += winItem.Cost;
			minFixBalance = minFixBalance > lootBox.Balance ? lootBox.Balance : minFixBalance;
			openingCosts += lootBox.Cost;
		}

		foreach(var lootBoxInventory in lootBox.Inventories!) 
		{
			var expectedChance = Math.Round(lootBoxInventory.ChanceWining / 100000M, 2);
			var expectedCount = Math.Round(iterations * expectedChance / 100, 2);
			var actualCount = winIds.Count(id => id == lootBoxInventory.ItemId);
			var actualChance = Math.Round(actualCount * 100M / iterations, 2);
			lootBoxInventoriesAndNumberDrops.Add(lootBoxInventory, actualCount);
		}

		foreach (var publishObject in publishObjects) 
		{
			if (publishObject is SiteStatisticsAdminTemplate sat)
			{
				revenue += sat.RevenueLootBoxCommission;
				fundsUsersInventories += sat.FundsUsersInventories;
			}
			if (publishObject is UserInventoryTemplate uit) 
			{
				inventoryCost += uit.FixedCost;
			}
		}

		var isMoneyLeak = openingCosts - revenue - fundsUsersInventories - lootBox.Balance != 0M;

		//Assert
		Assert.Equal(0, minFixBalance);
		Assert.Equal(revenueExpected, revenue);
		Assert.Equal(fundsUsersInventoriesExpected, fundsUsersInventories);
		Assert.Equal(fundsUsersInventoriesExpected, inventoryCost);
		Assert.False(isMoneyLeak);
	}

	[Theory]
	[ClassData(typeof(SimpleWithPromoCodesData))]
	[ClassData(typeof(SimpleWithPromoCodesExtendedData))]
	public async void OpenBox_PromoCodesData_SmallPriceWinItem(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);
		var isHasItem = imageDb.Data.LootBoxInventories.Any(lbi => lbi.BoxId == lootBox.Id && lbi.ItemId == winItem.Id);

		//Assert
		Assert.True(isHasItem);
		Assert.True(winItem.Cost < lootBox.Cost);
	}

	[Theory]
	[ClassData(typeof(SimpleWithPromoCodesData))]
	[ClassData(typeof(SimpleWithPromoCodesExtendedData))]
	public async void OpenBox_PromoCodesData_UserAndLootBoxBalance(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
		var promoCode = imageDb.Data.UserPromoCodes.First(upc => upc.UserId == userInfo.UserId);

		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var lootBoxCost = promoCode.Discount >= 0.99M ? 1 : lootBox.Cost * (1M - promoCode.Discount);
		var userBalanceExpected = userInfo.Balance - lootBoxCost;

		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var revenue = lootBoxCost * CommonConstants.RevenuePercentage;
		var lootBoxBalanceExpected = lootBoxCost - revenue - winItem.Cost;

		//Assert
		Assert.Equal(lootBoxBalanceExpected, lootBox.Balance);
		Assert.Equal(userBalanceExpected, userInfo.Balance);
	}

	[Theory]
	[ClassData(typeof(SimpleWithPromoCodesData))]
	[ClassData(typeof(SimpleWithPromoCodesExtendedData))]
	public async void OpenBox_PromoCodesData_PublishTemplates(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes[Random.Next(0, imageDb.Data.LootBoxes.Count)];
		var userInfos = imageDb.Data.UserAdditionalInfos.Where(uai => uai.Balance >= lootBox.Cost).ToList();
		var userInfo = userInfos[Random.Next(0, userInfos.Count)];
		var promoCode = imageDb.Data.UserPromoCodes.First(upc => upc.UserId == userInfo.UserId);
		
		var publishObjects = new List<object>();
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb, publishObjects);

		//Act
		var lootBoxCost = promoCode.Discount >= 0.99M ? 1 : lootBox.Cost * (1M - promoCode.Discount);
		var revenue = lootBoxCost * CommonConstants.RevenuePercentage;
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var statisticsAdminTemplate = new SiteStatisticsAdminTemplate();
		var userPromoCodeBackTemplate = new UserPromoCodeBackTemplate();

		foreach (var publishObject in publishObjects) 
		{
			if (publishObject is SiteStatisticsAdminTemplate sat)
			{
				statisticsAdminTemplate = sat;
			}
			else if (publishObject is UserPromoCodeBackTemplate ubt) 
			{
				userPromoCodeBackTemplate = ubt;
			}
		}

		//Assert
		Assert.Equal(revenue, statisticsAdminTemplate.RevenueLootBoxCommission);
		Assert.Equal(promoCode.Id, userPromoCodeBackTemplate.Id);
	}
	
	[Theory]
	[ClassData(typeof(SimpleWithPathBannersData))]
	[ClassData(typeof(SimpleWithPathBannersExtendedData))]
	public async void OpenBox_PathBannersData_SmallPriceWinItem(ImageDb imageDb)
	{
		//Arrange
		var pathBanners = imageDb.Data.UserPathBanners
			.Where(usp => 
				usp.NumberSteps > 1 && 
				usp.Box!.ExpirationBannerDate > DateTime.UtcNow &&
				imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == usp.UserId).Balance >= usp.Box.Cost)
			.ToList();
		var pathBanner = pathBanners[Random.Next(0, pathBanners.Count)];
		
		var lootBox = pathBanner.Box!;
		var userInfo = imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == pathBanner.UserId);
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var numberStepExpected = pathBanner.NumberSteps - 1;
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);
		var isHasItem = imageDb.Data.LootBoxInventories.Any(lbi => lbi.BoxId == lootBox.Id && lbi.ItemId == winItem.Id);

		//Assert
		Assert.True(isHasItem);
		Assert.True(winItem.Cost < lootBox.Cost);
		Assert.Equal(numberStepExpected, pathBanner.NumberSteps);
	}

	[Theory]
	[ClassData(typeof(SimpleWithPathBannersData))]
	[ClassData(typeof(SimpleWithPathBannersExtendedData))]
	public async void OpenBox_PathBannersData_PathBannerWinItem(ImageDb imageDb)
	{
		//Arrange
		var pathBanners = imageDb.Data.UserPathBanners
			.Where(usp => 
				usp.NumberSteps == 1 && 
				usp.Box!.ExpirationBannerDate > DateTime.UtcNow &&
				imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == usp.UserId).Balance >= usp.Box.Cost)
			.ToList();
		var pathBanner = pathBanners[Random.Next(0, pathBanners.Count)];
		
		var lootBox = pathBanner.Box!;
		var userInfo = imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == pathBanner.UserId);
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		//Assert
		Assert.Equal(pathBanner.ItemId, winItem.Id);
		Assert.Equal(0, pathBanner.NumberSteps);
	}

	[Theory]
	[ClassData(typeof(SimpleWithPathBannersData))]
	[ClassData(typeof(SimpleWithPathBannersExtendedData))]
	public async void OpenBox_PathBannersData_UserAndLootBoxBalanceWithRetention(ImageDb imageDb)
	{
		//Arrange
		var pathBanners = imageDb.Data.UserPathBanners
			.Where(usp => 
				usp.NumberSteps == 1 && 
				usp.Box!.ExpirationBannerDate > DateTime.UtcNow &&
				imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == usp.UserId).Balance >= usp.Box.Cost)
			.ToList();
		var pathBanner = pathBanners[Random.Next(0, pathBanners.Count)];
		
		var lootBox = pathBanner.Box!;
		var userInfo = imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == pathBanner.UserId);
		
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var userBalanceExpected = userInfo.Balance - lootBox.Cost;

		await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var revenue = lootBox.Cost * CommonConstants.RevenuePercentageBanner;
		var retentionBanner = lootBox.Cost * CommonConstants.RetentionPercentageBanner;
		var lootBoxBalanceExpected = lootBox.Cost - revenue - retentionBanner;

		//Assert
		Assert.Equal(lootBoxBalanceExpected, lootBox.Balance);
		Assert.Equal(userBalanceExpected, userInfo.Balance);
	}
	
	[Theory]
	[ClassData(typeof(SimpleWithPathBannersData))]
	[ClassData(typeof(SimpleWithPathBannersExtendedData))]
	public async void OpenBox_PathBannersData_UserAndLootBoxBalanceWithRetentionAndWinItem(ImageDb imageDb)
	{
		//Arrange
		var pathBanners = imageDb.Data.UserPathBanners
			.Where(usp => 
				usp.NumberSteps > 1 && 
				usp.Box!.ExpirationBannerDate > DateTime.UtcNow &&
				imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == usp.UserId).Balance >= usp.Box.Cost)
			.ToList();
		var pathBanner = pathBanners[Random.Next(0, pathBanners.Count)];
		
		var lootBox = pathBanner.Box!;
		var userInfo = imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == pathBanner.UserId);
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb);

		//Act
		var userBalanceExpected = userInfo.Balance - lootBox.Cost;

		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var revenue = lootBox.Cost * CommonConstants.RevenuePercentageBanner;
		var retentionBanner = lootBox.Cost * CommonConstants.RetentionPercentageBanner;
		var lootBoxBalanceExpected = lootBox.Cost - revenue - winItem.Cost - retentionBanner;

		//Assert
		Assert.Equal(lootBoxBalanceExpected, lootBox.Balance);
		Assert.Equal(userBalanceExpected, userInfo.Balance);
	}

	[Theory]
	[ClassData(typeof(SimpleWithPathBannersData))]
	[ClassData(typeof(SimpleWithPathBannersExtendedData))]
	public async void OpenBox_PathBannersData_PublishTemplates(ImageDb imageDb)
	{
		//Arrange
		var pathBanners = imageDb.Data.UserPathBanners
			.Where(usp => 
				usp.NumberSteps == 1 && 
				usp.Box!.ExpirationBannerDate > DateTime.UtcNow && 
				imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == usp.UserId).Balance >= usp.Box.Cost)
			.ToList();
		var pathBanner = pathBanners[Random.Next(0, pathBanners.Count)];
		
		var lootBox = pathBanner.Box!;
		var userInfo = imageDb.Data.UserAdditionalInfos.First(uai => uai.UserId == pathBanner.UserId);
		
		var publishObjects = new List<object>();
			
		var openingService = MockHelper.FillLootBoxOpeningService(imageDb, publishObjects);

		//Act
		await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id);

		var revenue = lootBox.Cost * CommonConstants.RevenuePercentageBanner;

		var statisticsAdminTemplate = new SiteStatisticsAdminTemplate();

		foreach (var publishObject in publishObjects) 
		{
			if (publishObject is SiteStatisticsAdminTemplate sat)
			{
				statisticsAdminTemplate = sat;
			}
		}

		//Assert
		Assert.Equal(revenue, statisticsAdminTemplate.RevenueLootBoxCommission);
	}

}