using System.Linq.Expressions;
using Game.BLL.Constants;
using Game.BLL.Interfaces;
using Game.BLL.Services;
using Game.UAT.Models;
using Game.UAT.TestData;
using Infrastructure.MassTransit.Statistics;
using Infrastructure.MassTransit.User;
using Microsoft.Extensions.Logging;
using Moq;

namespace Game.UAT.Tests.Integrations;
public class LootBoxOpeningTests
{
	[Theory]
	[ClassData(typeof(SimpleData))]
	[ClassData(typeof(SimpleExtendedData))]
	public async void OpenBox_WithStandardData_ShouldReturnSmallPriceWinItem(ImageDb imageDb)
	{
		//Arrange
		var lootBox = imageDb.Data.LootBoxes.First();
		var userInfo = imageDb.Data.UserAdditionalInfos.First(uai => uai.Balance >= lootBox.Balance);
		var gameItem = imageDb.Data.LootBoxInventories.OrderBy(lbi => lbi.Item!.Cost).First().Item!;

		var mockLogger = new Mock<ILogger<LootBoxOpeningService>>();
		var mockPublisher = new Mock<IBasePublisher>();
		var mockWrapperContext = new Mock<ApplicationDbContextWrapper>(imageDb.MockContext.Object);
		
		var publishObjects = new List<object>();

		var tokenSource = new CancellationTokenSource();
		tokenSource.CancelAfter(5000);

		mockWrapperContext.Setup(m => 
			m.SetEntryIsModifyProperty(
				It.IsAny<It.IsAnyType>(), 
				It.IsAny<Expression<Func<It.IsAnyType, decimal>>>(), 
				It.IsAny<bool>())).Callback(new InvocationAction(i => {}));
		mockPublisher.Setup(m => m.SendAsync(It.IsAny<It.IsAnyType>(), tokenSource.Token))
			.Callback(new InvocationAction(i => { publishObjects.Add(i.Arguments[0]); }));
			
		var openingService = new LootBoxOpeningService(
			imageDb.MockContext.Object, 
			mockLogger.Object, 
			mockPublisher.Object, 
			mockWrapperContext.Object);

		//Act
		var userBalanceExpected = userInfo.Balance - lootBox.Cost;

		var winItem = await openingService.OpenBoxAsync(userInfo.UserId, lootBox.Id, tokenSource.Token);

		var revenue = lootBox.Cost * CommonConstants.RevenuePercentage;
		var lootBoxBalanceExpected = lootBox.Cost - revenue - winItem.Cost;

		UserInventoryTemplate inventoryTemplate = new();
		SiteStatisticsTemplate statisticsTemplate = new();
		SiteStatisticsAdminTemplate statisticsAdminTemplate = new();

		foreach(var publishObject in publishObjects) 
		{
			if(publishObject is UserInventoryTemplate uit) 
				inventoryTemplate = uit;
			else if(publishObject is SiteStatisticsTemplate sst) 
				statisticsTemplate = sst;
			else if(publishObject is SiteStatisticsAdminTemplate sat)
				statisticsAdminTemplate = sat;
		}

		//Assert
		Assert.Equal(gameItem.Id, winItem.Id);
		Assert.Equal(lootBoxBalanceExpected, lootBox.Balance);
		Assert.Equal(winItem.Cost, inventoryTemplate.FixedCost);
		Assert.Equal(winItem.Id, inventoryTemplate.ItemId);
		Assert.Equal(userInfo.UserId, inventoryTemplate.UserId);
		Assert.Equal(1, statisticsTemplate.LootBoxes);
		Assert.Equal(0, statisticsTemplate.Reviews);
		Assert.Equal(0, statisticsTemplate.Users);
		Assert.Equal(0, statisticsTemplate.WithdrawnFunds);
		Assert.Equal(0, statisticsTemplate.WithdrawnItems);
		Assert.Equal(winItem.Cost, statisticsAdminTemplate.FundsUsersInventories);
		Assert.Equal(0, statisticsAdminTemplate.ReturnedFunds);
		Assert.Equal(revenue, statisticsAdminTemplate.RevenueLootBoxCommission);
		Assert.Equal(0, statisticsAdminTemplate.TotalReplenishedFunds);
		Assert.Equal(userBalanceExpected, userInfo.Balance);
	}

	//TODO CheckChancesTest
	//TODO UserPathBannerTest
	//TODO UserPromoCodesTest
}