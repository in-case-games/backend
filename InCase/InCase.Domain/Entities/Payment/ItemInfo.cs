using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Entities.Payment
{
    public class ItemInfo
    {
        public decimal Price { get; set; } = decimal.Zero;
        public int Count { get; set; }
        public GameItem Item { get; set; } = null!;
        public GamePlatform Platform { get; set; } = null!;
    }
}
