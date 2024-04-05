using Game.DAL.Entities;

namespace Game.UAT.Models;
public class DataDb
{
	public List<GameItem> GameItems { get; set; } = null!;
	public List<LootBoxInventory> LootBoxInventories { get; set; } = null!;
	public List<LootBox> LootBoxes { get; set; } = null!;
	public List<User> Users { get; set; } = null!;
	public List<UserAdditionalInfo> UserAdditionalInfos { get; set;} = null!;
	public List<UserOpening> UserOpenings { get; set; } = null!;
	public List<UserPathBanner> UserPathBanners { get; set; } = null!;
	public List<UserPromoCode> UserPromoCodes { get; set; } = null!;
}