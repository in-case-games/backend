namespace Infrastructure.Models;
public class GameMarket : BaseModel
{
    public string? Name { get; set; }
    public Guid GameId { get; set; }
}