using Resources.DAL.Entities;

namespace Resources.BLL.Models;
public class LootBoxGroupRequest : BaseEntity
{
    public Guid BoxId { get; set; }
    public Guid GameId { get; set; }
    public Guid GroupId { get; set; }
}