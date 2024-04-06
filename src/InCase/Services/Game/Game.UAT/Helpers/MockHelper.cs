using Game.DAL.Data;
using Game.UAT.Models;
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
}