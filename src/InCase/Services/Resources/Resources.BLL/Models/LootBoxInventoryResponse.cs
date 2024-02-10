using Resources.DAL.Entities;

namespace Resources.BLL.Models;
public class LootBoxInventoryResponse : BaseEntity
{
    public int ChanceWining { get; set; }
    public LootBoxResponse? Box { get; set; }
    public GameItemResponse? Item { get; set; }
}