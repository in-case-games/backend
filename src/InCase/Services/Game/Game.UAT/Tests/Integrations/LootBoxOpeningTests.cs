using Game.BLL.Constants;
using Game.UAT.Helpers;
using Game.UAT.Models;
using Game.UAT.TestData;
using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;

namespace Game.UAT.Tests.Integrations;
public class LootBoxOpeningTests
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

		UserInventoryTemplate inventoryTemplate = new();

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

		SiteStatisticsTemplate statisticsTemplate = new();

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

		SiteStatisticsAdminTemplate statisticsAdminTemplate = new();

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

		SiteStatisticsAdminTemplate statisticsAdminTemplate = new();
		UserPromoCodeBackTemplate userPromoCodeBackTemplate = new();

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
	public async void OpenBox_PathBannersData_UserAndLootBoxBalanceByRetention(ImageDb imageDb)
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
	public async void OpenBox_PathBannersData_UserAndLootBoxBalanceByRetentionAndWinItem(ImageDb imageDb)
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

		SiteStatisticsAdminTemplate statisticsAdminTemplate = new();

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

	//TODO CheckChancesTest
	//TODO Check Positive Balance
}