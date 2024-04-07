using System.Collections;
using Game.DAL.Entities;
using Game.UAT.Models;
using Game.UAT.Helpers;

namespace Game.UAT.TestData;
public class SimpleWithPromoCodesData : IEnumerable<object[]>
{
	private readonly List<GameItem> _gameItems = [
		new() { 
			Id = new Guid("0cf8b470-5f2c-4c63-a47a-1d2c1cb4e42b"), 
			Cost = 43662.5M
		},
		new() { 
			Id = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4"), 
			Cost = 8853.67M
		},
		new() { 
			Id = new Guid("3d79c3fc-9b1c-409a-a536-d9fe02fcdc20"), 
			Cost = 2369.36M
		},
	];
	private readonly List<LootBoxInventory> _lootBoxInventories = [
		new() { 
			ChanceWining = 410517, 
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9"), 
			ItemId = new Guid("0cf8b470-5f2c-4c63-a47a-1d2c1cb4e42b")
		},
		new() { 
			ChanceWining = 2024492, 
			BoxId = new Guid("b8df38f1-7043-4354-bf46-3ed8cf1b0de9"), 
			ItemId = new Guid("6726a162-abd1-4db3-be34-23c4b25754a4")
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
		},
	];
	private readonly List<User> _users = [ 
		new() { Id = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9") },
		new() { Id = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22") }
	];
	private readonly List<UserAdditionalInfo> _userAdditionalInfos = [ 
		new() { 
			Balance = int.MaxValue, 
			IsGuestMode = false, 
			UserId = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9")
		},
		new() { 
			Balance = 10000000, 
			IsGuestMode = false, 
			UserId = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22")
		}
	];
	private readonly List<UserOpening> _userOpenings = [];
	private readonly List<UserPathBanner> _userPathBanners = [];
	private readonly List<UserPromoCode> _userPromoCodes = [
		new() { Discount = 0.5M, UserId = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9") },
		new() { Discount = 0.3M, UserId = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22") }
	];

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