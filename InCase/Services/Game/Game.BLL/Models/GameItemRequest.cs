using Game.DAL.Entities;

namespace Game.BLL.Models
{
    public class GameItemRequest : BaseEntity
    {
        public decimal Cost { get; set; }
    }
}
