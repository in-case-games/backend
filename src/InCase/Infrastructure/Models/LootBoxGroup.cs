namespace Infrastructure.Models;
public class LootBoxGroup : BaseModel
{
    public Guid BoxId { get; set; }
    public Guid GroupId { get; set; }
    public Guid GameId { get; set; }
}