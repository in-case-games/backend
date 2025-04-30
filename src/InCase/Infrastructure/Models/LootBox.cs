namespace Infrastructure.Models;
public class LootBox : BaseModel
{
    public string? Name { get; set; }
    public decimal Cost { get; set; }
    public bool IsLocked { get; set; } = true;
    public Guid GameId { get; set; }
}