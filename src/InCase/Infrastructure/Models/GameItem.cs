namespace Infrastructure.Models;
public class GameItem : BaseModel
{
    public string? Name { get; set; }
    public string? HashName { get; set; }
    public string? IdForMarket { get; set; }
    public decimal Cost { get; set; }

    public Guid GameId { get; set; }
    public Guid TypeId { get; set; }
    public Guid RarityId { get; set; }
    public Guid QualityId { get; set; }
}