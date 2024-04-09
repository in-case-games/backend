using System.Linq.Expressions;
using Game.BLL.Interfaces;
using Game.BLL.Services;
using Game.DAL.Data;
using Game.UAT.Models;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Game.UAT.Helpers;
public class MockHelper
{
	public static Mock<ApplicationDbContext> FillDataMockApplicationDbContext(DataDb data) 
	{
		var mockContext = new Mock<ApplicationDbContext>();
		var gameItemsMockSet = data.GameItems.AsQueryable().BuildMockDbSet();
		var lootBoxInventoriesMockSet = data.LootBoxInventories.AsQueryable().BuildMockDbSet();
		var lootBoxesMockSet = data.LootBoxes.AsQueryable().BuildMockDbSet();
		var usersMockSet = data.Users.AsQueryable().BuildMockDbSet();
		var userAdditionalInfosMockSet = data.UserAdditionalInfos.AsQueryable().BuildMockDbSet();
		var userOpeningsMockSet = data.UserOpenings.AsQueryable().BuildMockDbSet();
		var userPathBannersMockSet = data.UserPathBanners.AsQueryable().BuildMockDbSet();
		var userPromoCodesMockSet = data.UserPromoCodes.AsQueryable().BuildMockDbSet();

		mockContext.Setup(m => m.GameItems).Returns(gameItemsMockSet.Object);
		mockContext.Setup(m => m.LootBoxes).Returns(lootBoxesMockSet.Object);
		mockContext.Setup(m => m.LootBoxInventories).Returns(lootBoxInventoriesMockSet.Object);
		mockContext.Setup(m => m.Users).Returns(usersMockSet.Object);
		mockContext.Setup(m => m.UserAdditionalInfos).Returns(userAdditionalInfosMockSet.Object);
		mockContext.Setup(m => m.UserOpenings).Returns(userOpeningsMockSet.Object);
		mockContext.Setup(m => m.UserPathBanners).Returns(userPathBannersMockSet.Object);
		mockContext.Setup(m => m.UserPromoCodes).Returns(userPromoCodesMockSet.Object);

		return mockContext;
	}

	public static void FillSetupsApplicationDbContextWrapper(Mock<ApplicationDbContextWrapper> mockWrapperContext) 
	{
		mockWrapperContext.Setup(m => 
			m.SetEntryIsModifyProperty(
				It.IsAny<It.IsAnyType>(), 
				It.IsAny<Expression<Func<It.IsAnyType, decimal>>>(), 
				It.IsAny<bool>())).Callback(new InvocationAction(i => {}));
	}

	public static LootBoxOpeningService FillLootBoxOpeningService(
		ImageDb imageDb, 
		List<object>? publishObjects = default, 
		CancellationTokenSource? tokenSource = default) 
	{
		var mockLogger = new Mock<ILogger<LootBoxOpeningService>>();
		var mockPublisher = new Mock<IBasePublisher>();
		var mockWrapperContext = new Mock<ApplicationDbContextWrapper>(imageDb.MockContext.Object);
		
		FillSetupsApplicationDbContextWrapper(mockWrapperContext);

		var token = tokenSource?.Token ?? new CancellationToken();

		mockPublisher.Setup(m => m.SendAsync(It.IsAny<It.IsAnyType>(), token))
			.Callback(new InvocationAction(i => { publishObjects?.Add(i.Arguments[0]); }));
			
		return new LootBoxOpeningService(
			imageDb.MockContext.Object, 
			mockLogger.Object, 
			mockPublisher.Object, 
			mockWrapperContext.Object);
	}
}