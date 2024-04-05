using System.Collections;
using Game.DAL.Entities;
using Game.UAT.Helpers;
using Game.UAT.Models;

namespace Game.UAT.TestData;
public class SimpleExtendedData : IEnumerable<object[]>
{
	private static readonly List<GameItem> _gameItems = [
		new() { 
			Id = new Guid("2d3156ee-4fab-45f7-97d9-a94875f32182"), 
			Cost = 3186.19M 
		},
		new() { 
			Id = new Guid("26033696-a831-4202-a7df-eaf97554ce5f"),
			Cost = 9415 
		},
		new() { 
			Id = new Guid("15c1405f-dab9-45f4-a497-d87a1e65596a"),
			Cost = 50581.58M 
		},
	];
	private static readonly List<LootBoxInventory> _lootBoxInventories = [
		new() { 
			ChanceWining = 2414832, 
			BoxId = new Guid("8a5ba229-0a21-4154-9855-8e44e0dd647d"), 
			ItemId = new Guid("2d3156ee-4fab-45f7-97d9-a94875f32182")
		},
		new() { 
			ChanceWining = 449485,
			BoxId = new Guid("8a5ba229-0a21-4154-9855-8e44e0dd647d"),
			ItemId = new Guid("26033696-a831-4202-a7df-eaf97554ce5f")
		},
		new() { 
			ChanceWining = 7135683,
			BoxId = new Guid("8a5ba229-0a21-4154-9855-8e44e0dd647d"),
			ItemId = new Guid("15c1405f-dab9-45f4-a497-d87a1e65596a")
		}
	];
	private static readonly List<LootBox> _lootBoxes = [ 
		new() { 
			Id = new Guid("8a5ba229-0a21-4154-9855-8e44e0dd647d"), 
			Cost = 5000,
		} 
	];
	private static readonly List<User> _users = [ 
		new() { Id = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9") },
		new() { Id = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22") }
	];
	private static readonly List<UserAdditionalInfo> _userAdditionalInfos = [ 
		new() { 
			Balance = 10000000000, 
			IsGuestMode = false, 
			UserId = new Guid("44e233f0-ec99-4e56-9df4-8ca8761b86b9")
		},
		new() { 
			Balance = 10000000, 
			IsGuestMode = false, 
			UserId = new Guid("c56a68d2-5759-462c-aa02-ba9151cfac22")
		}
	];
	private static readonly List<UserOpening> _userOpenings = [];
	private static readonly List<UserPathBanner> _userPathBanners = [];
	private static readonly List<UserPromoCode> _userPromoCodes = [];

	public IEnumerator<object[]> GetEnumerator()
	{
		for(var i = 0; i < _lootBoxInventories.Count; i++) {
			_lootBoxInventories[i].Item = _gameItems[i];
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