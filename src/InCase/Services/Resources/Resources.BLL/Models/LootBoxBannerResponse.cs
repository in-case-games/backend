using Resources.DAL.Entities;

namespace Resources.BLL.Models;

public class LootBoxBannerResponse : BaseEntity
{
    public DateTime CreationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }

    public LootBoxResponse? Box { get; set; }
}