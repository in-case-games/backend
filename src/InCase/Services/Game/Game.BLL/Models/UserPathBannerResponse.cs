using Game.DAL.Entities;

namespace Game.BLL.Models;
public class UserPathBannerResponse : BaseEntity
{
    public int NumberSteps { get; set; }
    public decimal FixedCost { get; set; }

    public GameItem? Item { get; set; }
    public LootBox? Box { get; set; }
}