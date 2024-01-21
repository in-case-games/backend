using Resources.DAL.Entities;

namespace Resources.BLL.Models;

public class GameItemRequest : BaseEntity
{
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? IdForMarket { get; set; }
    public string? HashName { get; set; }
    public decimal Cost { get; set; }

    public Guid GameId { get; set; }
    public Guid TypeId { get; set; }
    public Guid RarityId { get; set; }
    public Guid QualityId { get; set; }
}