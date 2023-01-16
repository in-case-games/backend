namespace wdskills.DomainLayer.Entities
{
    public class GameItem : BaseEntity
    {
        public string? GameItemName { get; set; }
        public decimal GameItemCost { get; set; }
        public string? GameItemImage { get; set; }
        public string? GameItemRarity { get; set; }
    }
}
