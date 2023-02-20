using System.ComponentModel;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemBuyOfferTM
    {
        [DisplayName("c")]
        public string? Count { get; set; }
        [DisplayName("my_count")]
        public string? MyCount { get; set; }
        [DisplayName("o_price")]
        public string? Price { get; set; }
    }
}
