using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Models
{
    public class BuyItemResponse
    {
        public string Id { get; set; } = null!;
        public GameMarket Market { get; set; } = null!;
    }
}
