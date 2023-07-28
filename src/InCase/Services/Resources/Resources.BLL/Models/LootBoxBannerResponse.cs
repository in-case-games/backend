using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Entities
{
    public class LootBoxBannerResponse : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public LootBoxResponse? Box { get; set; }
    }
}
