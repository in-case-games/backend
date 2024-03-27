using Resources.DAL.Entities;

namespace Resources.BLL.Models;
public class LootBoxBannerRequest : BaseEntity
{
    public string? Image { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid BoxId { get; set; }
}