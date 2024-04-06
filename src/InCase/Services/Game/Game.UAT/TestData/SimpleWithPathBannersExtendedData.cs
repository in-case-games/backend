using System.Collections;
using Game.DAL.Entities;
using Game.UAT.Helpers;
using Game.UAT.Models;

namespace Game.UAT.TestData;
public class SimpleWithPathBannersExtendedData : IEnumerable<object[]>
{
	private readonly List<GameItem> _gameItems = [
		new() { 
			Id = new Guid("e3e192e7-9696-46ab-a98a-6737ba48b866"), 
			Cost = 7
		},
		new() { 
			Id = new Guid("3d79c3fc-9b1c-409a-a536-d9fe02fcdc20"), 
			Cost = 2369.36M
		},
		new() { 
			Id = new Guid("6d4b6cca-2ba5-4caa-8a1f-ef7372957a59"), 
			Cost = 4633.72M
		},
		new() { 
			Id = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4"), 
			Cost = 8853.67M
		},
		new() { 
			Id = new Guid("85630b60-4f92-4992-92c5-935082407631"), 
			Cost = 16519.37M
		},
		new() { 
			Id = new Guid("0cf8b470-5f2c-4c63-a47a-1d2c1cb4e42b"), 
			Cost = 43662.5M
		},
		new() { 
			Id = new Guid("14b5cb08-4387-4228-9df2-d40ca748c22a"), 
			Cost = 333365.76M
		},
		new() { 
			Id = new Guid("2217549f-2ead-4096-9743-814f5e1d766a"), 
			Cost = 447038.27M
		}
	];
	private readonly List<LootBoxInventory> _lootBoxInventories = [
		new() { 
			ChanceWining = 27394, 
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			ItemId = new Guid("2217549f-2ead-4096-9743-814f5e1d766a")
		},
		new() { 
			ChanceWining = 36735, 
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			ItemId = new Guid("14b5cb08-4387-4228-9df2-d40ca748c22a")
		},
		new() { 
			ChanceWining = 410517, 
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9"), 
			ItemId = new Guid("0cf8b470-5f2c-4c63-a47a-1d2c1cb4e42b")
		},
		new() { 
			ChanceWining = 741320, 
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			ItemId = new Guid("85630b60-4f92-4992-92c5-935082407631")
		},
		new() { 
			ChanceWining = 1383172, 
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4")
		},
		new() { 
			ChanceWining = 2024492, 
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9"), 
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4")
		},
		new() { 
			ChanceWining = 2642833, 
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			ItemId = new Guid("6d4b6cca-2ba5-4caa-8a1f-ef7372957a59")
		},
		new() { 
			ChanceWining = 5168546, 
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			ItemId = new Guid("3d79c3fc-9b1c-409a-a536-d9fe02fcdc20")
		},
		new() { 
			ChanceWining = 7564991, 
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9"), 
			ItemId = new Guid("3d79c3fc-9b1c-409a-a536-d9fe02fcdc20")
		},
	];
	private readonly List<LootBox> _lootBoxes = [ 
		new() { 
			Id = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9"), 
			Cost = 4000,
			ExpirationBannerDate = DateTime.UtcNow.AddYears(90)
		},
		new() { 
			Id = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e"), 
			Cost = 6000,
			ExpirationBannerDate = DateTime.UtcNow.AddYears(90)
		} 
	];
	private readonly List<User> _users = [ 
		new() { Id = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9") },
		new() { Id = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22") },
		new() { Id = new Guid("c44ed294-e63d-438c-ac7e-7d7f6bb58b24") }
	];
	private readonly List<UserAdditionalInfo> _userAdditionalInfos = [ 
		new() { 
			Balance = 10000000000, 
			IsGuestMode = false, 
			UserId = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9")
		},
		new() { 
			Balance = 10000000, 
			IsGuestMode = false, 
			UserId = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22")
		},
		new() { 
			Balance = 0, 
			IsGuestMode = false, 
			UserId = new Guid("c44ed294-e63d-438c-ac7e-7d7f6bb58b24")
		}
	];
	private readonly List<UserOpening> _userOpenings = [];
	private readonly List<UserPathBanner> _userPathBanners = [
		new() {
			NumberSteps = 1,
			FixedCost = 8800,
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4"),
			UserId = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9"),
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9")
		},
		new() {
			NumberSteps = 2,
			FixedCost = 4400,
			ItemId = new Guid("6d4b6cca-2ba5-4caa-8a1f-ef7372957a59"),
			UserId = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9"),
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e")
		},
		new() {
			NumberSteps = 1,
			FixedCost = 8700,
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4"),
			UserId = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22"),
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e")
		},
		new() {
			NumberSteps = 2,
			FixedCost = 9300,
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4"),
			UserId = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22"),
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9")
		},
		new() {
			NumberSteps = 1,
			FixedCost = 7115,
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4"),
			UserId = new Guid("c44ed294-e63d-438c-ac7e-7d7f6bb58b24"),
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9")
		},
		new() {
			NumberSteps = 2,
			FixedCost = 17000,
			ItemId = new Guid("85630b60-4f92-4992-92c5-935082407631"),
			UserId = new Guid("c44ed294-e63d-438c-ac7e-7d7f6bb58b24"),
			BoxId = new Guid("891bd4c1-1a3f-435f-b7af-5e68207a905e")
		}
	];
	private readonly List<UserPromoCode> _userPromoCodes = [];

	public IEnumerator<object[]> GetEnumerator()
	{
		for (var i = 0; i < _userPathBanners.Count; i++) 
		{
			var id = _userPathBanners[i].BoxId;
			_userPathBanners[i].Box = _lootBoxes.First(lb => lb.Id == id);
		}
		for(var i = 0; i < _lootBoxInventories.Count; i++) {
			var id = _lootBoxInventories[i].ItemId;
			_lootBoxInventories[i].Item = _gameItems.First(gi => gi.Id == id);
		}
		for(var i = 0; i < _lootBoxes.Count; i++) {
			var id = _lootBoxes[i].Id;
			_lootBoxes[i].Inventories = _lootBoxInventories.Where(lbi => lbi.BoxId == id);
		}

		var dataDb = new DataDb() {
			GameItems = _gameItems,
			LootBoxInventories = _lootBoxInventories,
			LootBoxes = _lootBoxes,
			Users = _users,
			UserAdditionalInfos = _userAdditionalInfos,
			UserOpenings = _userOpenings,
			UserPathBanners = _userPathBanners,
			UserPromoCodes = _userPromoCodes
		};
		var mockContext = MockHelper.FillDataMockApplicationDbContext(dataDb);

		yield return new object[] {
			new ImageDb
			{
				MockContext = mockContext,
				Data = dataDb
			},
		};
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}