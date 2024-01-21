using Resources.DAL.Entities;

namespace Resources.BLL.Models;

public class LootBoxGroupResponse : BaseEntity
{
    public GroupLootBox? Group { get; set; }
    public LootBoxResponse? Box { get; set; }
    public GameResponse? Game { get; set; }
}