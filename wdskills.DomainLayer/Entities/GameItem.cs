namespace CaseApplication.DomainLayer.Entities
{
    public class GameItem : BaseEntity
    {
        public string? GameItemName { get; set; }
        public decimal GameItemCost { get; set; }
        public string? GameItemImage { get; set; }
        public string? GameItemRarity { get; set; }
        public ICollection<CaseInventory>? CaseInventories { get; set; }
        public ICollection<UserInventory>? UserInventories { get; set; }
    }
}
