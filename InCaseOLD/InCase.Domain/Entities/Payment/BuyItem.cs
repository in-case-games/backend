using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Entities.Payment
{
    public class BuyItem
    {
        public string? Id { get; set; }
        public string? Result { get; set; }
        public GameMarket? Market { get; set; }
    }
}
