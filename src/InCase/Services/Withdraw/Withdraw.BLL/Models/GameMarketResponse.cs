using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models
{
    public class GameMarketResponse : BaseEntity
    {
        public string? Name { get; set; }
        public Game? Game { get; set; }
    }
}
