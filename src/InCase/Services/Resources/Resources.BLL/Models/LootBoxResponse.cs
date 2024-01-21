using Resources.DAL.Entities;

namespace Resources.BLL.Models;

public class LootBoxResponse : BaseEntity
{
    public string? Name { get; set; }
    public decimal Cost { get; set; }
    public bool IsLocked { get; set; }
    public string? Game { get; set; }
}