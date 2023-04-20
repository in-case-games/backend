using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Entities.Payment
{
    public class TradeInfo
    {
        public int Id { get; set; }
        public string Status { get; set; } = "";
        public GameItem? Item { get; set; }
    }
}
