using Game.DAL.Entities;

namespace Game.BLL.Models;

public class UserPathBannerRequest : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
    public Guid BoxId { get; set; }
}