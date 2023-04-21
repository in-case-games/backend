using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Entities.Payment
{
    public class ItemInfo
    {
        public int PriceKopecks { get; set; } = 0;
        public int Count { get; set; }
        public GameItem Item { get; set; } = null!;
        public GameMarket Market { get; set; } = null!;
    }
}
