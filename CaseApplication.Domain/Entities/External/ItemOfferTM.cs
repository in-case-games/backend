using System.ComponentModel;

namespace CaseApplication.Domain.Entities.External
{
    public class ItemOfferTM
    {
        [DisplayName("price")]
        public string? Price { get; set; }
        [DisplayName("count")]
        public string? Count { get; set; }
        [DisplayName("my_count")]
        public string? MyCount { get; set; }
    }
}
