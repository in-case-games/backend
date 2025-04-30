namespace Infrastructure.Models;
public class LootBoxInventory : BaseModel
{
    public int ChanceWining { get; set; }
    public GameItem Item { get; set; } = new();
    public LootBox Box { get; set; } = new();
}